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
            try
            {
                var fichajes = await _service.GetAllAsync();
                return Ok(fichajes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PostFichajeDTO dto)
        {
            try
            {
                await _service.CrearFichajeAsync(dto);
                return Ok(new { message = "Fichaje creado para el usuario:" + dto.IdUsuario.ToString() + " de " + dto.TipoFichaje.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
           
        }
        

     [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostFichajeDTO dto)
        {
            try
            {
                var actualizado = await _service.ActualizarAsync(id, dto);

                if (!actualizado)
                    return NotFound(new { message = "Fichaje no encontrado" });

                return Ok(new { message = "Fichaje actualizado para el usuario:" + dto.IdUsuario.ToString() + " de " + dto.TipoFichaje.ToString() });
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
                return Ok(new { message = "Fichaje nº " + id + " eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
           
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetListaFichajeUsuario(int id)
        {
            try
            {
                var fichajes = await _service.GetListaFichajePorUsuarioAsync(id);
                return Ok(fichajes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
          
        }

        [HttpGet("usuario/{id}/ultimo")]
        public async Task<IActionResult> GetUltimoFichajeUsuario(int id)
        {
            try
            {
                var fichaje = await _service.GetUltimoFichajePorUsuarioAsync(id);
                return Ok(fichaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorIdFichaje(int id)
        {
            try
            {
                var fichaje = await _service.GetPorIdFichajeAsync(id);
                return Ok(fichaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetListaUsuarios()
        {
            try
            {
                var usuarios = await _service.GetUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno");
            }
            
        }
    } 
}
