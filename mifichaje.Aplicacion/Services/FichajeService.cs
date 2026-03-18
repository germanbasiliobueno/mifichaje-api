using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mifichaje.Aplicacion.DTOs;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;

namespace mifichaje.Aplicacion.Services
{
    public class FichajeService 
    {
        private readonly IFichajeRepository _repository;
        

        public FichajeService(IFichajeRepository repository)
        {
            _repository = repository;
        }
      
        public async Task<IEnumerable<FichajeDTO>> GetAllAsync()
        {
            var fichajes = await _repository.ListaTotalAsync();
            return fichajes.Select(e => new FichajeDTO { IdFichaje = e.IdFichaje, IdUsuario = e.IdUsuario, NombreUsuario = e.NombreUsuario, FechaHora = e.FechaHora, TipoFichaje = (TipoFichaje)e.TipoFichaje, Latitud = e.Latitud, Longitud = e.Longitud, Calle = e.Calle, Ciudad = e.Ciudad, FechaRegistro = e.FechaRegistro });

        }


        public async Task CrearFichajeAsync(PostFichajeDTO dto)
        {
            
            await _repository.AñadirAsync(dto);
        }

        public async Task<bool> ActualizarAsync(int id, PostFichajeDTO dto)
        {
            var tipo = (TipoFichaje)dto.TipoFichaje;
            var Fichaje = new Fichaje { IdFichaje = id, IdUsuario = dto.IdUsuario, TipoFichaje = (int)dto.TipoFichaje, Latitud = dto.Latitud, Longitud =dto.Longitud, Calle = dto.Calle, Ciudad = dto.Ciudad };
            var actualizado = await _repository.ActualizarAsync(id, Fichaje);
            return actualizado;
        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }

        public async Task<IEnumerable<FichajeDTO>> GetListaFichajePorUsuarioAsync(int id)
        {
            var fichajes = await _repository.ListaFichajePorUsuarioAsync(id);
            return fichajes.Select(e => new FichajeDTO { IdFichaje = e.IdFichaje, IdUsuario = e.IdUsuario, NombreUsuario = e.NombreUsuario, FechaHora = e.FechaHora, TipoFichaje = (TipoFichaje)e.TipoFichaje, Latitud = e.Latitud, Longitud = e.Longitud, Calle = e.Calle, Ciudad = e.Ciudad, FechaRegistro = e.FechaRegistro });

        }

        public async Task<FichajeDTO> GetUltimoFichajePorUsuarioAsync(int idusuario)
        {
            var fichaje = await _repository.ObtenerUltimoFichajePorUsuarioAsync(idusuario);
            var fichajeDTO = new FichajeDTO { IdFichaje = fichaje.IdFichaje, IdUsuario = fichaje.IdUsuario,  FechaHora = fichaje.FechaHora, TipoFichaje = (TipoFichaje)fichaje.TipoFichaje, Latitud = fichaje.Latitud, Longitud = fichaje.Longitud, Calle = fichaje.Calle, Ciudad = fichaje.Ciudad, FechaRegistro = fichaje.FechaRegistro };
            return fichajeDTO;          
        }

        public async Task<FichajeDTO?> GetPorIdFichajeAsync(int idfichaje)
        {
            var fichaje = await _repository.ObtenerPorIdFichajeAsync(idfichaje);
            if (fichaje == null) return null;

            return new FichajeDTO
            {
                IdFichaje = fichaje.IdFichaje,
                IdUsuario = fichaje.IdUsuario,
                NombreUsuario = fichaje.NombreUsuario,
                FechaHora = fichaje.FechaHora,
                TipoFichaje = fichaje.TipoFichaje,
                Latitud = fichaje.Latitud,
                Longitud = fichaje.Longitud, 
                Calle = fichaje.Calle,
                Ciudad = fichaje.Ciudad,
                FechaRegistro = fichaje.FechaRegistro
            };
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUsuariosAsync()
        {
            var usuarios = await _repository.ListaUsuariosAsync();
            return usuarios.Select(e => new UsuarioDTO { IdUsuario = e.IdUsuario, Documento = e.Documento, NombreUsuario = e.NombreUsuario, Correo = e.Correo, Clave = e.Clave, IdRol = e.IdRol, Descripcion = e.Descripcion, Estado = e.Estado, FechaRegistro = e.FechaRegistro });

        }

    }
}
