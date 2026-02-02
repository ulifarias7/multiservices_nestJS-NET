using Auth.API.MappingConfig;
using Auth.API.Middleware;
using Auth.API.Services;
using Auth.API.Services.Implementation;
using Keycloak.Net;

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

//http client keycloack
builder.Services.AddHttpClient<KeycloackServices>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Keycloak:ServerUrl"]!);
}); 

//services 
builder.Services.AddScoped<IKeycloackServices, KeycloackServices>();

//wrapper
builder.Services.AddScoped<KeycloakWrapper>();

//automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "local")
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//logs del kestrel que expone el puerto 
app.Lifetime.ApplicationStarted.Register(() =>
    {
        var addresses = app.Urls;
        foreach (var address in addresses)
        {
            app.Logger.LogInformation($"Aplicación escuchando en: {address}");
        }
    }
);

app.UseMiddleware<KeycloakExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthorized();
app.MapControllers();
app.Run();
