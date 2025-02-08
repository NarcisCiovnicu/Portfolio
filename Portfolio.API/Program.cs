using Portfolio.API;
using Portfolio.API.AppLogic;
using Portfolio.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

AppLogicSetup.Initialize(app.Services);

app.UseRateLimiter();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
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
