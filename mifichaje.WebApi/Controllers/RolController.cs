using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Services;


namespace mifichaje.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController(RolService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListaTotalAll()
        {
            try
            {
                var Rols = await _service.GetAllAsync();
                return Ok(Rols);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PostRolDTO dto)
        {
            try
            {
                await _service.AñadirAsync(dto);
                return Ok(new { message = "Rol creado con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutRolDTO dto)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, dto);

                if (!actualizado)
                    return NotFound(new { message = "Rol no encontrado" });

                return Ok(new { message = "Rol actualizado" });
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
                return Ok(new { message = "Rol eliminado" });
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
                var rol = await _service.GetPorIdAsync(id);
                if (rol == null) return NotFound();
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
           
        }
    }
}
