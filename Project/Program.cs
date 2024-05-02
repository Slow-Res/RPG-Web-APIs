
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using Project.Controllers;
global using Project.Data;
global using Project.DTOs.Character;
global using Project.Services.CharacterServices;
global using Project.Models;
global using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Project.Services.WeaponServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>( opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = @"Standard Auth header using the bearer scheme",
        In = ParameterLocation.Header,
        Name= "Authorization",
        Type= SecuritySchemeType.ApiKey
    }) ;
    c.OperationFilter<SecurityRequirementsOperationFilter>(); 
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterServices, CharacterService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IWeaponService, WeaponService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:AccessTokenSeceret").Value!)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
