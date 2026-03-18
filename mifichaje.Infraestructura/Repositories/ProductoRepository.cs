using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Aplicacion.Services;
using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;


namespace mifichaje.Infraestructura.Repositories
{
    public class ProductoRepository(DBConnectionFactory _connection) : IProductoRepository
    {

        public async Task<IEnumerable<Producto>> ListaTotalAsync()
        {
            var productos = new List<Producto>();
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "SELECT IdProducto, Nombre, Precio, FechaRegistro FROM Producto";

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                productos.Add(new Producto
                {
                    IdProducto = Convert.ToInt32(reader["IdProducto"].ToString()),
                    Nombre = reader["Nombre"].ToString(),
                    Precio = Convert.ToDecimal(reader["Precio"].ToString()),
                    FechaRegistro = reader["FechaRegistro"].ToString(),
                });
            }

            return productos;
        }

        public async Task AñadirAsync(Producto producto)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "INSERT INTO Producto (Nombre, Precio) VALUES (@Nombre, @Precio)";
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Producto producto)
        {

            try
            {
                using var comm = _connection.CreateConnection();
                using var cmd = comm.CreateCommand();

                cmd.CommandText = "UPDATE Producto SET Nombre=@Nombre,Precio=@Precio WHERE IdProducto = @IdProducto";
                cmd.Parameters.AddWithValue("@IdProducto", id);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);



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

            cmd.CommandText = "DELETE FROM Producto WHERE IdProducto = @IdProducto";
            cmd.Parameters.AddWithValue("@IdProducto", id);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }


        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"
        SELECT IdProducto, Nombre, Precio, FechaRegistro
        FROM Producto
        WHERE IdProducto = @IdProducto";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdProducto";
            param.Value = id;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new Producto
            {
                IdProducto = reader["IdProducto"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdProducto"]),
                Nombre = reader["Nombre"]?.ToString(),
                Precio = reader["Precio"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Precio"]),
                FechaRegistro = reader["FechaRegistro"]?.ToString()
            };
        }

    }
}

