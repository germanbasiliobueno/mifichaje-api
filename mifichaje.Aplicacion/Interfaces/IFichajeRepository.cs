using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.DTOs;
using mifichaje.Dominio;

namespace mifichaje.Aplicacion.Interfaces
{
    public interface IFichajeRepository
    {
        Task<IEnumerable<FichajeDTO>> ListaTotalAsync();
        Task AñadirAsync(PostFichajeDTO fichaje);
        Task<bool> ActualizarAsync(int id, Fichaje fichaje);
        Task EliminarAsync(int idfichaje);
        Task<FichajeDTO> ObtenerUltimoFichajePorUsuarioAsync(int idUsuario);
        Task<List<FichajeDTO>> ListaFichajePorUsuarioAsync(int idUsuario);
        Task<FichajeDTO> ObtenerPorIdFichajeAsync(int idfichaje);
        Task<IEnumerable<UsuarioDTO>> ListaUsuariosAsync();



        //  Task PostFichajeAsync(PostFichajeDTO dto);

        //    Task GuardarCambiosAsync();
    }
}
