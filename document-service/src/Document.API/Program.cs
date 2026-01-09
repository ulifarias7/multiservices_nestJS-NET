using Document.API.Common.Middleware;
using Document.API.Database.Persistence;
using Document.API.Messaging;
using Document.API.Repository;
using Document.API.Repository.Implementations;
using Document.API.Services;
using Document.API.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Minio;

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

//rabbitmq
var rabbitOptions = builder.Configuration
    .GetSection("RabbitMQ")
    .Get<RabbitMqOptions>()!;

builder.Services.AddSingleton(rabbitOptions);
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
builder.Services.AddHostedService(sp =>
    (RabbitMqPublisher)sp.GetRequiredService<IRabbitMqPublisher>()
);

//services
builder.Services.AddScoped<IFileServices, FileServices>();

//repository
builder.Services.AddScoped<IFileRepository,FileRepository>();

//Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DocumentService")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
