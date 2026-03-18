using mifichaje.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.DTOs;

namespace mifichaje.Aplicacion.Interfaces
{
    public interface IUsuarioRepository
    {

        Task<IEnumerable<UsuarioDTO>> ListaTotalAsync();
        Task AñadirAsync(Usuario usuario);
        Task<bool> ActualizarAsync(int id, Usuario usuario);
        Task EliminarAsync(int id);
        Task<Usuario> ObtenerPorIdAsync(int id);
    }
}
