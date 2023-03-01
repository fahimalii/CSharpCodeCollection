using ExplorePolly.Api.Clients;
using Polly;
using Polly.Contrib.WaitAndRetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();

//builder.Services.AddHttpClient(name: "openweatherapi", client =>
//{
//    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
//});

//builder.Services.AddHttpClient(name: "openweatherapi",
//    client =>
//    {
//        client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
//    })
//    .AddPolicyHandler(Policy<HttpResponseMessage>
//    .Handle<HttpRequestException>()
//    .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
//    .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));


// TODO: Check logging onRetry
builder.Services.AddHttpClient(name: "openweatherapi",
    client =>
    {
        client.BaseAddress = new Uri("https://api.openweather.org/data/2.5/");
        //client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
    })
    .AddTransientHttpErrorPolicy(policyBuilder =>
    {
        return policyBuilder
            .WaitAndRetryAsync(
                sleepDurations: Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)
                //onRetry: (DelegateResult<HttpResponseMessage> outcome, TimeSpan timespan, int retryAttempt, Context context) =>
                //{
                //    //builder.Services.GetService<ILogger<OpenWeatherClient>>()?
                //    //    .LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                //    context.GetLogger()?.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                //}
                );
        //onRetryAsync: (outcome, timespan, retryAttempt, context) =>
        //{
        //    context.GetLogger()?.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
        //    return Task.CompletedTask;
        //});
    });


var app = builder.Build();

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
