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
            var Rols = await _service.GetAllAsync();
            return Ok(Rols);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PostRolDTO dto)
        {
            await _service.AñadirAsync(dto);
            return Ok(new { message = "Rol creado con éxito" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutRolDTO dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { message = "Rol no encontrado" });

            return Ok(new { message = "Rol actualizado" });

           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { message = "Rol eliminado" });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var rol = await _service.GetPorIdAsync(id);
            if (rol == null) return NotFound();
            return Ok(rol);
        }
    }
}
