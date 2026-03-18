using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;
using mifichaje.Aplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Infraestructura.Repositories
{
    public class UsuarioRepository(DBConnectionFactory _connection) : IUsuarioRepository
    {
        public async Task<IEnumerable<UsuarioDTO>> ListaTotalAsync()
        {
            var usuarios = new List<UsuarioDTO>();
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "SELECT u.IdUsuario, u.Documento, u.NombreUsuario, u.Correo, u.Clave, u.IdRol, r.Descripcion, u.Estado, u.FechaRegistro FROM Usuario u INNER JOIN ROL r ON u.IdRol = r.IdRol";

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                usuarios.Add(new UsuarioDTO
                {
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                    Documento = reader["Documento"].ToString(),
                    NombreUsuario = reader["NombreUsuario"].ToString(),
                    Correo = reader["Correo"].ToString(),
                    Clave = reader["Clave"].ToString(),
                    IdRol = Convert.ToInt32(reader["IdRol"].ToString()),
                    Descripcion = reader["Descripcion"].ToString(),
                    Estado = Convert.ToBoolean(reader["Estado"].ToString()), 
                    FechaRegistro = reader["FechaRegistro"].ToString()


                });
            }

            return usuarios;
        }

        public async Task AñadirAsync(Usuario usuario)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "INSERT INTO Usuario (Documento, NombreUsuario, Correo, Clave,IdRol, Estado) VALUES (@Documento, @NombreUsuario, @Correo, @Clave, @IdRol, @Estado)";
            cmd.Parameters.AddWithValue("@Documento", usuario.Documento);
            cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
            cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
            cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Usuario usuario)
        {

            try
            {
                using var comm = _connection.CreateConnection();
                using var cmd = comm.CreateCommand();

                cmd.CommandText = "UPDATE Usuario SET Documento=@Documento, NombreUsuario=@NombreUsuario, Correo=@Correo, Clave=@Clave, IdRol=@IdRol, Estado=@Estado, FechaRegistro = GETDATE() WHERE IdUsuario = @IdUsuario";
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cmd.Parameters.AddWithValue("@Documento", usuario.Documento);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario );
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

                await comm.OpenAsync();
                await cmd.ExecuteReaderAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task EliminarAsync(int id)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "DELETE FROM Usuario WHERE IdUsuario = @IdUsuario";
            cmd.Parameters.AddWithValue("@IdUsuario", id);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }


        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"
        SELECT IdUsuario,Documento, NombreUsuario, Correo, Clave, IdRol, Estado, FechaRegistro
        FROM Usuario
        WHERE IdUsuario = @IdUsuario";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdUsuario";
            param.Value = id;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new Usuario
            {
                IdUsuario = reader["IdUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUsuario"]),
                Documento = reader["Documento"]?.ToString(),
                NombreUsuario = reader["NombreUsuario"]?.ToString(),
                Correo = reader["Correo"]?.ToString(),
                Clave = reader["Clave"]?.ToString(),
                IdRol = reader["IdRol"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRol"]),
                Estado = Convert.ToBoolean(reader["Estado"].ToString()),
                FechaRegistro = reader["FechaRegistro"].ToString(),

            };
        }


    }
}
