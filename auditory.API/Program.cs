using Auditory.API.Database.Persistence;
using Auditory.API.Messaging.Consumers;
using Auditory.API.Messaging.Options;
using Auditory.API.Repository;
using Auditory.API.Repository.Implementations;
using Auditory.API.Services;
using Auditory.API.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//controllers
builder.Services.AddControllers();

//services
builder.Services.AddScoped<IAuditServices, AuditServices>();

//repository
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//contexto
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuditService"));
});

//rabbit 
var rabbitOptions = builder.Configuration
    .GetSection("RabbitMQ")
    .Get<RabbitMqDocumentServiceOptions>()!;

builder.Services.AddSingleton(rabbitOptions);
builder.Services.AddHostedService<DocumentCreatedConsumer>();

//build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "local")
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

////Log URL del swagger
//var logger = app.Services.GetRequiredService<ILogger<Program>>();
//var httpPort = builder.Configuration["Swagger:HTTP-PORT"];
//var httpsPort = builder.Configuration["Swagger:HTTPS-PORT"];
//var port = httpPort ?? httpsPort;
//logger.LogInformation($"Swagger disponible en: http://localhost:{port}/swagger", port);


//logs del kestrel que expone el puerto 
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Urls;
    foreach (var address in addresses)
    {
        app.Logger.LogInformation($"Aplicaci�n escuchando en: {address}");
    }
}
);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
