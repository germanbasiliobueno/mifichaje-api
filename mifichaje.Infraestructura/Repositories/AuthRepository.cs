using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;


namespace mifichaje.Infraestructura.Repositories
{
    public class AuthRepository(DBConnectionFactory _connection) : IAuthRepository
    {

        public async Task<Usuario> UsuarioRolPorDocumentoAsync(string documento)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "SELECT u.IdUsuario, u.Documento, u.NombreUsuario, u.Correo, u.Clave, u.IdRol, u.Estado,u.FechaRegistro, r.Descripcion AS Rol FROM dbo.USUARIO u LEFT JOIN dbo.ROL r ON r.IdROL = u.IdRol WHERE u.Documento = @Documento";

            var param = cmd.CreateParameter();
            param.ParameterName = "@Documento";
            param.Value = documento;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;
           
                return new Usuario
                {
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                    Documento = reader["Documento"].ToString(),
                    NombreUsuario = reader["NombreUsuario"].ToString(),
                    Correo = reader["Correo"].ToString(),
                    Clave = reader["Clave"].ToString(),
                    IdRol = Convert.ToInt32(reader["IdRol"].ToString()),
                    Estado = Convert.ToBoolean(reader["Estado"].ToString()),
                    FechaRegistro = reader["FechaRegistro"].ToString(),
                };              
        }
       
        public async Task<Rol> RolPorIdRolAsync(int idrol)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "select IdROL, Descripcion, FechaRegistro from rol where IdRol = @idrol";

            var param = cmd.CreateParameter();
            param.ParameterName = "@idrol";
            param.Value = idrol;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new Rol
            {
                IdRol = Convert.ToInt32(reader["IdRol"].ToString()),
                Descripcion = reader["Descripcion"].ToString(),
                FechaRegistro = reader["FechaRegistro"].ToString(),
            };

        }
    }
}
