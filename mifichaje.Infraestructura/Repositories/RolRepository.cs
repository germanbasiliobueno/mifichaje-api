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
    public class RolRepository : BaseRepository, IRolRepository
    {
        public RolRepository(DBConnectionFactory connection) : base(connection) { }
        public async Task<IEnumerable<Rol>> ListaTotalAsync()
        {
            var rols = new List<Rol>();
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT IdRol, Descripcion, FechaRegistro FROM Rol";


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
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO Rol (Descripcion) VALUES (@Descripcion)";
            cmd.Parameters.AddWithValue("@Descripcion", rol.Descripcion);

            await cmd.ExecuteReaderAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Rol rol)
        {

            try
            {
                using var conn = await OpenConnectionAsync();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = "UPDATE Rol SET Descripcion=@Descripcion,  FechaRegistro = GETDATE() WHERE IdRol = @IdRol";
                cmd.Parameters.AddWithValue("@IdRol", id);
                cmd.Parameters.AddWithValue("@Descripcion", rol.Descripcion);

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
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Rol WHERE IdRol = @IdRol";
            cmd.Parameters.AddWithValue("@IdRol", id);

            await cmd.ExecuteReaderAsync();
        }


        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            using var conn = await OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
        SELECT IdRol, Descripcion, FechaRegistro
        FROM Rol
        WHERE IdRol = @IdRol";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdRol";
            param.Value = id;
            cmd.Parameters.Add(param);

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
