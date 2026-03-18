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
   public  class RolService 
    {
        private readonly IRolRepository _repository;
        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RolDTO>> GetAllAsync()
        {
            var rols = await _repository.ListaTotalAsync();
            return rols.Select(e => new RolDTO { IdRol = e.IdRol, Descripcion = e.Descripcion, FechaRegistro = e.FechaRegistro });

        }
        public async Task AñadirAsync(PostRolDTO dto)
        {
            var rol = new Rol { Descripcion = dto.Descripcion };
            await _repository.AñadirAsync(rol);
        }

        public async Task<bool> ActualizarAsync(int id, PutRolDTO dto)
        {
            var Rol = new Rol { IdRol = id, Descripcion = dto.Descripcion};
            var actualizado = await _repository.ActualizarAsync(id, Rol);
            return actualizado;

        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }

        public async Task<RolDTO?> GetPorIdAsync(int id)
        {
            var rol = await _repository.ObtenerPorIdAsync(id);
            if (rol == null) return null;

            return new RolDTO
            {
                IdRol = rol.IdRol,
                Descripcion = rol.Descripcion,
                FechaRegistro = rol.FechaRegistro
            };
        }

    }
}
