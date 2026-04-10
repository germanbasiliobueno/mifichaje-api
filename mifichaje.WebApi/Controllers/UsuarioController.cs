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
            try
            {
                var Usuarios = await _service.GetAllAsync();
                return Ok(Usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioPostDTO dto)
        {
            try
            {
                await _service.AñadirAsync(dto);
                return Ok(new { message = "Usuario creado con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            await _service.AñadirAsync(dto);
            return Ok(new { message = "Usuario creado con éxito" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioPutDTO dto)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, dto);

                if (!actualizado)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = "Usuario actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return Ok(new { message = "Usuario eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            try
            {
                var usuario = await _service.GetPorIdAsync(id);
                if (usuario == null) return NotFound();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }          
        }
    }
}
