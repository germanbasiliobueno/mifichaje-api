using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Dominio;


namespace mifichaje.Aplicacion.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> ListaTotalAsync();
        Task AñadirAsync(Rol rol);
        Task<bool> ActualizarAsync(int id, Rol rol);
        Task EliminarAsync(int id);
        Task<Rol> ObtenerPorIdAsync(int id);
    }
}
