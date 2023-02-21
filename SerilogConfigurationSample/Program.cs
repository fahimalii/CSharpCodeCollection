using Serilog;
using SerilogConfigurationSample;
using SerilogConfigurationSample.LogUtility;
using SerilogConfigurationSample.Middleware;
using SerilogConfigurationSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISampleService, SampleService>();
builder.Services.AddScoped<CorelationIdProvider>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseMiddleware<CorelationIdProviderMiddleware>();

//  Method to introduce automatic HTTP request logging in your API.
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
