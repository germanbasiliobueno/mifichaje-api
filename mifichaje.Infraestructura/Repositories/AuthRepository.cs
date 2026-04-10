using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;


namespace mifichaje.Infraestructura.Repositories
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(DBConnectionFactory connection) : base(connection) { }

        public async Task<Usuario?> UsuarioRolPorDocumentoAsync(string documento)
        {
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT u.IdUsuario, u.Documento, u.NombreUsuario, u.Correo, u.Clave, u.IdRol, u.Estado,u.FechaRegistro, r.Descripcion AS Rol FROM dbo.USUARIO u LEFT JOIN dbo.ROL r ON r.IdROL = u.IdRol WHERE u.Documento = @Documento";
            cmd.Parameters.AddWithValue("@Documento", documento);

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                Documento = reader["Documento"].ToString(),
                NombreUsuario = reader["NombreUsuario"].ToString(),
                Correo = reader["Correo"].ToString(),
                Clave = reader["Clave"].ToString(),
                IdRol = Convert.ToInt32(reader["IdRol"]),
                Estado = Convert.ToBoolean(reader["Estado"])
            };
        }

        public async Task<Rol> RolPorIdRolAsync(int idrol)
        {
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "select IdROL, Descripcion, FechaRegistro from rol where IdRol = @idrol";
            cmd.Parameters.AddWithValue("@idrol", idrol);
       
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

