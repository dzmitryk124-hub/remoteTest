using RemoteTest.Core.Interfaces;
using RemoteTest.Core.Services;
using RemoteTest.Database;
using RemoteTest.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MeterReadingDbContext>();
builder.Services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddScoped<ICsvMeterReadingParser, CsvMeterReadingParser>();
builder.Services.AddScoped<IMeterReadingValidator, MeterReadingValidator>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
