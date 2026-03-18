using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.Interfaces;

namespace mifichaje.Infraestructura.Repositories
{
    public class RolRepository(DBConnectionFactory _connection) : IRolRepository
    {
        public async Task<IEnumerable<Rol>> ListaTotalAsync()
        {
            var rols = new List<Rol>();
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "SELECT IdRol, Descripcion, FechaRegistro FROM Rol";

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rols.Add(new Rol
                {
                    IdRol = Convert.ToInt32(reader["IdRol"].ToString()),
                    Descripcion = reader["Descripcion"].ToString(),
                    FechaRegistro = reader["FechaRegistro"].ToString(),
                });
            }

            return rols;
        }

        public async Task AñadirAsync(Rol rol)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "INSERT INTO Rol (Descripcion) VALUES (@Descripcion)";
            cmd.Parameters.AddWithValue("@Descripcion", rol.Descripcion);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Rol rol)
        {

            try
            {
                using var comm = _connection.CreateConnection();
                using var cmd = comm.CreateCommand();

                cmd.CommandText = "UPDATE Rol SET Descripcion=@Descripcion,  FechaRegistro = GETDATE() WHERE IdRol = @IdRol";
                cmd.Parameters.AddWithValue("@IdRol", id);
                cmd.Parameters.AddWithValue("@Descripcion", rol.Descripcion);

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

            cmd.CommandText = "DELETE FROM Rol WHERE IdRol = @IdRol";
            cmd.Parameters.AddWithValue("@IdRol", id);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }


        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"
        SELECT IdRol, Descripcion, FechaRegistro
        FROM Rol
        WHERE IdRol = @IdRol";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdRol";
            param.Value = id;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new Rol
            {
                IdRol = reader["IdRol"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRol"]),
                Descripcion = reader["Descripcion"]?.ToString(),
                FechaRegistro = reader["FechaRegistro"]?.ToString()
            };
        }


        
    }


}
