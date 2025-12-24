using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Room.Me.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;

        public JwtService(IConfiguration config)
        {
            _jwtKey = config["Jwt:Key"];
            if(_jwtKey == null)
            {
                throw new Exception("La clave JWT no está configurada");
            }
        }

        //Metodo para generar el token JWT
        public string GenerateToken(int userId, string email)
        {
            //Crear clave con el Key de las variables de entorno
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtKey)
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
                issuer: "RoomMeAPI",
                audience: "RoomMeAPIUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
