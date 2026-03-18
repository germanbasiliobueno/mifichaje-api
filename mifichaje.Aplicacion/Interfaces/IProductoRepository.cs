using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Dominio;

namespace mifichaje.Aplicacion.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> ListaTotalAsync();
    Task AñadirAsync(Producto producto);
    Task<bool> ActualizarAsync(int id, Producto producto);
    Task EliminarAsync(int id);
    Task<Producto> ObtenerPorIdAsync(int id);



}
