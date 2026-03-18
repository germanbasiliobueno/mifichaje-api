using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Services;

namespace mitienda.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(ProductoService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListaTotalAll()
        {
            var Productos = await _service.GetAllAsync();
            return Ok(Productos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody]PostProductoDTO dto)
        {
            await _service.AñadirAsync(dto);
            return Ok(new { message = "Producto creado con éxito"});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutProductoDTO dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { message = "Producto no encontrado" });

            return Ok(new { message = "Producto actualizado" });

            /* await _service.ActualizarAsync(id, dto);
             return Ok(new { message = "Producto actualizado" });
            */
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { message = "Producto eliminado" });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var producto = await _service.GetPorIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }
    }
}
