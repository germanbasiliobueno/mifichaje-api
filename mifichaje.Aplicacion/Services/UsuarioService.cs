using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Aplicacion.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _repository.ListaTotalAsync();
            return usuarios.Select(e => new UsuarioDTO { IdUsuario = e.IdUsuario, Documento = e.Documento, NombreUsuario = e.NombreUsuario, Correo = e.Correo, Clave = e.Clave, IdRol = e.IdRol, Descripcion = e.Descripcion, Estado = e.Estado, FechaRegistro = e.FechaRegistro });

        }
        public async Task AñadirAsync(UsuarioPostDTO dto)
        {
            var usuario = new Usuario { Documento = dto.Documento, NombreUsuario = dto.NombreUsuario, Correo = dto.Correo, Clave = dto.Clave, IdRol = dto.IdRol, Estado = dto.Estado };
            await _repository.AñadirAsync(usuario);
        }

        public async Task<bool> ActualizarAsync(int id, UsuarioPutDTO dto)
        {
            var usuario = new Usuario { IdUsuario = id, Documento = dto.Documento, NombreUsuario = dto.NombreUsuario, Correo = dto.Correo, Clave = dto.Clave, IdRol = dto.IdRol, Estado = dto.Estado };
            var actualizado = await _repository.ActualizarAsync(id, usuario);
            return actualizado;

        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }

        public async Task<UsuarioDTO?> GetPorIdAsync(int id)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                Documento = usuario.Documento,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Clave = usuario.Clave,
                IdRol = usuario.IdRol,
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro
            };
        }




    }
}
