using Sbo.AndEmiliTest.ApiServer;
using Sbo.AndEmiliTest.Core;
using Sbo.AndEmiliTest.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAndEmiliTestDbContextAndFactory();
builder.Services.AddSingleton<BallDontLieServices>();
builder.Services.AddHostedService<Initializer>();

builder.Services.AddTransient<HttpClient>();
builder.Services.AddLogging(x => x.AddSimpleConsole());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
