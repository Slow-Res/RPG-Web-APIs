
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using Project.Controllers;
global using Project.Data;
global using Project.DTOs.Character;
global using Project.Services.CharacterServices;
global using Project.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>( opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterServices, CharacterService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
