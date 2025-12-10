using Auth.API.Services;
using Auth.API.Services.Implementation;
using Flurl;
using Keycloak.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//controllers 
builder.Services.AddControllers();

// Keycloak Client
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var url = config["Keycloak:ServerUrl"];
    var adminUser = config["Keycloak:AdminUsername"];
    var adminPass = config["Keycloak:AdminPassword"];
    var adminRealm = "master";

    return new KeycloakClient(url, adminUser, adminPass, new KeycloakOptions(authenticationRealm: adminRealm));
});


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
