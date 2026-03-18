using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Services;

namespace mitienda.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(UsuarioService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListaTotalAll()
        {
            var Usuarios = await _service.GetAllAsync();
            return Ok(Usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioPostDTO dto)
        {
            await _service.AñadirAsync(dto);
            return Ok(new { message = "Usuario creado con éxito" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioPutDTO dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(new { message = "Usuario actualizado" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { message = "Usuario eliminado" });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var usuario = await _service.GetPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }
    }
}
