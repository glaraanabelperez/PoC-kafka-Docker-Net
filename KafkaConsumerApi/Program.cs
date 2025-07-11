using KafkaConsumerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddSingleton<KafkaBackgroundConsumerService>();


//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8082);
//});

builder.Services.AddSingleton<KafkaBackgroundConsumerService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<KafkaBackgroundConsumerService>());


// Registrar la política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker" || app.Environment.IsProduction())
{
    Console.WriteLine("inicio app: " + app.Environment.EnvironmentName);
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
