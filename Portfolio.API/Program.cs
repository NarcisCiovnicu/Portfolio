using Portfolio.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupWebApi(builder.Configuration);
builder.Services.AddCorsPolicies(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseRateLimiter();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
