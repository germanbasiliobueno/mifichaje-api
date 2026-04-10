using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Dominio;


namespace mifichaje.Aplicacion.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _repository;
        public AuthService(IAuthRepository repository)
        {
            _repository = repository;
        }

        public async Task<LoginResponseDTO?> GetPorIDocumentoAsync(string documento)
        {
            var usuario = await _repository.UsuarioRolPorDocumentoAsync(documento);
            if (usuario == null) return null;

            var rol = await _repository.RolPorIdRolAsync(usuario.IdRol);
            if (rol == null) return null;

            if (usuario == null) return null;
            if (rol == null) return null;

            return new LoginResponseDTO
            {
                IdUsuario = usuario.IdUsuario,
                Documento = usuario.Documento,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Clave = usuario.Clave,
                Estado = usuario.Estado,
                  Descripcion = rol.Descripcion,
                IdRol = usuario.IdRol,
            };           
        }
    }
}
