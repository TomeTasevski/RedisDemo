using RedisDemo.Cache.Contracts;
using RedisDemo.Cache.Services;
using RedisDemo.Cache;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ICacheService, RedisCache>();
builder.Services.AddSingleton<IRedisCacheConfiguration, RedisCacheConfiguration>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
