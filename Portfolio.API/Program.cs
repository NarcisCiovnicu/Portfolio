using Portfolio.API;
using Portfolio.API.AppLogic;
using Portfolio.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogging();

/// MUST be added here in this file for Azure deployment to generate swagger.json
/// Otherwise it will skip that saying "AddSwaggerGen was not detected" if it's called from a different file
/// �\_(�_�)_/�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

AppLogicSetup.Initialize(app.Services);

app.UseRateLimiter();
app.UseHttpLogging();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<TrackingMiddleware>();
app.UseExceptionHandler();

await app.RunAsync();
