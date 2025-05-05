using API.Extensions;
using API.middleware;
using Microsoft.AspNetCore.RateLimiting; 
using System.Threading.RateLimiting; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAppServices(builder.Configuration); // This now includes Rate Limiter config

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add execution time middleware
app.UseExecutionTime();

// Add Rate Limiter middleware HERE
app.UseRateLimiter();

// Add authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers().RequireRateLimiting("fixed"); // Apply the rate limiting policy to all controllers (global)

app.Run();


