using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Room.Me.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        //Metodo para generar el token JWT
        public string GenerateToken(int userId, string email)
        {
            //Crear clave con el Key del appsettings.json
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            //Crear las credenciales 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Guardar información que se va a guardar en el token
            var claims = new[]
            {
                new Claim("id", userId.ToString()),
                new Claim("email", email)
            };

            //Configurar el token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
