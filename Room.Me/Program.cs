using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Room.Me.Data;
using Room.Me.Services;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Constructor para el servicio de envío de emails
builder.Services.AddSingleton<SendgidEmailServices>();
//Base de datos

var conn = builder.Configuration.GetConnectionString("DefaultConnection");


if (string.IsNullOrWhiteSpace(conn))
{
    throw new Exception("Connection string no configurada");
}

builder.Services.AddDbContext<RoomMeDbContext>(options =>
    options.UseSqlServer(conn)
);


//configuración de CORS para coneccion con el front
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


//Configuración de JWT

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "RoomMeAPI",
            ValidAudience = "RoomMeAPIUsers",

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                )
            )
        };
    });

builder.Services.AddScoped<JwtService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowVue");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
