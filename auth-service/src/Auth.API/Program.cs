using Auth.API.Services;
using Auth.API.Services.Implementation;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//controllers 
builder.Services.AddControllers();

//services 
builder.Services.AddScoped<IKeycloackServices, KeycloackServices>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
