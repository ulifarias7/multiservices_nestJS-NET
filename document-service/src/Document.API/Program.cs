using Document.API.Common.Middleware;
using Document.API.Database.Persistence;
using Document.API.Repository;
using Document.API.Repository.Implementations;
using Document.API.Services;
using Document.API.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Minio;
using RabbitMq.Channel;
using RabbitMq.Configurations;
using RabbitMq.Connection;
using RabbitMq.Models;
using RabbitMq.Publisher;
using RabbitMq.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//controllers
builder.Services.AddControllers();

//minio
var minioConfig = builder.Configuration.GetSection("Minio");

// MinIO Client
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    return new MinioClient()
        .WithEndpoint(minioConfig["Endpoint"])
        .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
        .WithSSL(bool.Parse(minioConfig["UseSSL"]!))
        .Build();
});

//services
builder.Services.AddScoped<IFileServices, FileServices>();

//repository
builder.Services.AddScoped<IFileRepository,FileRepository>();

//Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DocumentService")));


//rabbitmq
var rabbitMqUri = builder.Configuration["RabbitMQ:Url"];

builder.Services.AddSingleton<IRabbitMQConnectionFactory>(sp =>
{
    var connectionSettings = new ConnectionSettings
    {
        Url = rabbitMqUri
    };
    return new RabbitMQConnectionFactory(connectionSettings);
});

builder.Services.AddSingleton<IChannelManager, ChannelManager>();
builder.Services.AddSingleton<IMessageSerializer, MessageSerializer>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


//configuracion de la cola al iniciar la app
try
{
    using var scope = app.Services.CreateScope();
    var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

    await publisher.DeclareQueueAsync(new QueueConfiguration
    {
        QueueName = "document.created",
        Durable = true,
        Exclusive = true,
        AutoDelete = true
    });

    Console.WriteLine("✅ Cola 'document.created' declarada exitosamente");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error al declarar cola: {ex.Message}");
}

//middleware 
app.UseMiddleware<ExcetionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Log URL del swagger
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var httpPort = builder.Configuration["Swagger:HTTP-PORT"];
var httpsPort = builder.Configuration["Swagger:HTTPS-PORT"];
var port = httpPort ?? httpsPort;
logger.LogInformation("Swagger disponible en: http://localhost:{port}/swagger", port);

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.Run();
