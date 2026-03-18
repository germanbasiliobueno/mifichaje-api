using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Services;

namespace mifichaje.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly IConfiguration _config;

        public AuthController(AuthService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO req)
        {
            if (string.IsNullOrWhiteSpace(req.Documento) || string.IsNullOrWhiteSpace(req.Clave))
                return BadRequest("Documento y clave son obligatorios.");

            // ✅ Traemos el usuario por documento (y si puedes, que venga también RolDescripcion y Estado)
            var user = await _service.GetPorIDocumentoAsync(req.Documento);
            

            // Si no existe
            if (user == null || string.IsNullOrWhiteSpace(user.Documento))
                return Unauthorized("Credenciales incorrectas.");

            // ✅ Validar Estado (necesitas que el DTO lo traiga)
            // Si tu DTO no tiene Estado, añade Estado al DTO o crea un DTO específico para auth
            if (user.Estado == false)
                return Unauthorized("Usuario deshabilitado.");

            // ✅ Validar clave (AHORA MISMO la tienes en claro: user.Clave)
            // Recomendación: migrar a hash, pero por ahora:
            if (user.Clave != req.Clave)
                return Unauthorized("Credenciales incorrectas.");

            // ✅ Rol para claim (ideal: user.RolDescripcion, si no, al menos idRol)
       //     var rol = !string.IsNullOrWhiteSpace(user.RolDescripcion)
        //        ? user.RolDescripcion
        //        : $"ROL_{user.IdRol}";

            var token = CrearJwt(user.IdUsuario, user.Documento);

            return Ok(new LoginResponseDTO
            {
                IdUsuario = user.IdUsuario,
                Documento = user.Documento,
                NombreUsuario = user.NombreUsuario,
                Correo = user.Correo,
       //         Clave = user.Clave,
                Estado = user.Estado,
                IdRol = user.IdRol,
                Descripcion = user.Descripcion,
                AccessToken = token
            });
        }

        private string CrearJwt(int idUsuario, string documento)
        {
            var jwt = _config.GetSection("Jwt");

            var keyText = jwt["Key"];
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];
            var expireText = jwt["ExpireMinutes"];

            if (string.IsNullOrWhiteSpace(keyText))
                throw new InvalidOperationException("Falta Jwt:Key en appsettings.json");
            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("Falta Jwt:Issuer en appsettings.json");
            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("Falta Jwt:Audience en appsettings.json");

            var expireMinutes = 120;
            if (!string.IsNullOrWhiteSpace(expireText) && int.TryParse(expireText, out var parsed))
                expireMinutes = parsed;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Name, documento),
            //    new Claim(ClaimTypes.Role, rol ?? "SIN_ROL")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
