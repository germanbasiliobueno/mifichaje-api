using Microsoft.AspNetCore.Mvc;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Services;

namespace mifichaje.WebApi.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class FichajeController(FichajeService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> ListaTotalAll()
        {
            var fichajes = await _service.GetAllAsync();
            return Ok(fichajes);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PostFichajeDTO dto)
        {
            await _service.CrearFichajeAsync(dto);
            return Ok(new { message = "Fichaje creado para el usuario:" + dto.IdUsuario.ToString() + " de " + dto.TipoFichaje.ToString() });
        }
        

     [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostFichajeDTO dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { message = "Fichaje no encontrado" });

            return Ok(new { message = "Fichaje actualizado para el usuario:" + dto.IdUsuario.ToString() + " de " + dto.TipoFichaje.ToString() });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { message = "Fichaje nº " + id + " eliminado" });
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetListaFichajeUsuario(int id)
        {
            var fichajes = await _service.GetListaFichajePorUsuarioAsync(id);
            return Ok(fichajes);
        }

        [HttpGet("usuario/{id}/ultimo")]
        public async Task<IActionResult> GetUltimoFichajeUsuario(int id)
        {
            var fichaje = await _service.GetUltimoFichajePorUsuarioAsync(id);
            return Ok(fichaje);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorIdFichaje(int id)
        {
            var fichaje = await _service.GetPorIdFichajeAsync(id);
            return Ok(fichaje);
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetListaUsuarios()
        {
            var usuarios = await _service.GetUsuariosAsync();
            return Ok(usuarios);
        }
    } 
}
