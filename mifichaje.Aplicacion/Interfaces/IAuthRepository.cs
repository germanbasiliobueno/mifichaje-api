using mifichaje.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Aplicacion.Interfaces
{
    public interface IAuthRepository
    {
        Task<Usuario> UsuarioRolPorDocumentoAsync(string documento);
        Task<Rol> RolPorIdRolAsync(int idrol);
    //    Task<IEnumerable<Usuario>> ListaTotalAsync();
    //    Task AñadirAsync(Usuario Usuario);
    //    Task<bool> ActualizarAsync(int id, Usuario Usuario);
    //     Task EliminarAsync(int id);
    //     Task<Rol> ObtenerPorIdAsync(int id);

    }
}
