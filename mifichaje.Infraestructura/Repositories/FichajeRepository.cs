using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Dominio;
using mifichaje.Infraestructura.Database;
using mifichaje.Aplicacion.Interfaces;
using mifichaje.Aplicacion.DTOs;

using mifichaje.Aplicacion.Interfaces;

namespace mifichaje.Infraestructura.Repositories
{
    public class FichajeRepository(DBConnectionFactory _connection) : IFichajeRepository
    {
        public async Task<IEnumerable<FichajeDTO>> ListaTotalAsync()
        {
            var Fichajes = new List<FichajeDTO>();
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "select f.IdFichaje, f.IdUsuario, u.NombreUsuario, f.FechaHora AT TIME ZONE 'UTC' AT TIME ZONE 'Romance Standard Time' AS FechaHora, f.TipoFichaje, f.Latitud, f.Longitud, f.Calle, f.Ciudad, f.FechaRegistro  FROM FICHAJES f INNER JOIN USUARIO U ON f.IdUsuario = u.IdUsuario ORDER BY f.FechaHora DESC";

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Fichajes.Add(new FichajeDTO
                {
                    IdFichaje = Convert.ToInt32(reader["IdFichaje"].ToString()),
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                    NombreUsuario = reader["NombreUsuario"].ToString(),
                    FechaHora = Convert.ToDateTime(reader["FechaHora"].ToString()),
                    TipoFichaje = (TipoFichaje)Convert.ToByte(reader["TipoFichaje"]),
                    Latitud = Convert.ToDouble(reader["Latitud"].ToString()),
                    Longitud = Convert.ToDouble(reader["Longitud"].ToString()),
                    Calle = reader["Calle"].ToString(),
                    Ciudad = reader["Ciudad"].ToString(),
                    FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString()),
                });
            }

            return Fichajes;
        }

        public async Task AñadirAsync(PostFichajeDTO fichaje)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "INSERT INTO FICHAJES (IdUsuario,  TipoFichaje, Latitud, Longitud, Calle, Ciudad) VALUES (@IdUsuario, @TipoFichaje, @Latitud, @Longitud, @Calle, @Ciudad)";
            cmd.Parameters.AddWithValue("@IdUsuario", fichaje.IdUsuario);
            cmd.Parameters.AddWithValue("@TipoFichaje", fichaje.TipoFichaje);
            cmd.Parameters.AddWithValue("@Latitud", fichaje.Latitud);
            cmd.Parameters.AddWithValue("@Longitud", fichaje.Longitud);
            cmd.Parameters.AddWithValue("@Calle", fichaje.Calle);
            cmd.Parameters.AddWithValue("@Ciudad", fichaje.Ciudad);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Fichaje fichaje)
        {

            try
            {
                using var comm = _connection.CreateConnection();
                using var cmd = comm.CreateCommand();

                cmd.CommandText = "UPDATE FICHAJES SET IdUsuario = @IdUsuario, FechaHora = GETDATE(), TipoFichaje=@TipoFichaje, Latitud = @Latitud, Longitud = @Longitud, Calle = @Calle, Ciudad = @Ciudad, FechaRegistro = GETDATE() WHERE IdFichaje = @IdFichaje";
                cmd.Parameters.AddWithValue("@IdFichaje", id);
                cmd.Parameters.AddWithValue("@IdUsuario", fichaje.IdUsuario);
                cmd.Parameters.AddWithValue("@TipoFichaje", fichaje.TipoFichaje);
                cmd.Parameters.AddWithValue("@Latitud", fichaje.Latitud);
                cmd.Parameters.AddWithValue("@Longitud", fichaje.Longitud);
                cmd.Parameters.AddWithValue("@Calle", fichaje.Calle);
                cmd.Parameters.AddWithValue("@Ciudad", fichaje.Ciudad);

                await comm.OpenAsync();
                await cmd.ExecuteReaderAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task EliminarAsync(int idfichaje)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = "DELETE FROM FICHAJES WHERE IdFichaje = @IdFichaje";
            cmd.Parameters.AddWithValue("@IdFichaje", idfichaje);

            await comm.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }


        public async Task<FichajeDTO> ObtenerUltimoFichajePorUsuarioAsync(int idUsuario)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"
                 SELECT TOP 1  f.IdFichaje, f.IdUsuario, u.NombreUsuario, f.FechaHora AT TIME ZONE 'UTC' AT TIME ZONE 'Romance Standard Time' AS FechaHora, f.TipoFichaje, f.Latitud, f.Longitud, f.Calle, f.Ciudad
                 FROM FICHAJES f  INNER JOIN USUARIO u ON f.IdUsuario= u.IdUsuario 
                 WHERE f.IdUsuario = @IdUsuario ORDER BY f.FechaHora DESC 
";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdUsuario";
            param.Value = idUsuario;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new FichajeDTO
            {
                IdFichaje = reader["IdFichaje"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdFichaje"]),
                IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                NombreUsuario = reader["NombreUsuario"].ToString(),
                FechaHora = Convert.ToDateTime(reader["FechaHora"].ToString()),
                TipoFichaje = (TipoFichaje)Convert.ToByte(reader["TipoFichaje"]),
                Latitud = Convert.ToDouble(reader["Latitud"].ToString()),
                Longitud = Convert.ToDouble(reader["Longitud"].ToString()),
                Calle = reader["Calle"].ToString(),
                Ciudad = reader["Ciudad"].ToString()

            };
        }

        public async Task<List<FichajeDTO>> ListaFichajePorUsuarioAsync(int idUsuario)
        {
            var Fichajes = new List<FichajeDTO>();
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"select f.IdFichaje, f.IdUsuario, u.NombreUsuario, f.FechaHora AT TIME ZONE 'UTC' AT TIME ZONE 'Romance Standard Time' AS FechaHora, f.TipoFichaje, f.Latitud, f.Longitud, f.Calle, f.Ciudad, f.FechaRegistro  FROM FICHAJES f INNER JOIN USUARIO U ON f.IdUsuario = u.IdUsuario WHERE f.IdUsuario = @IdUsuario ORDER BY f.FechaHora DESC";
            var param = cmd.CreateParameter();
            param.ParameterName = "@IdUsuario";
            param.Value = idUsuario;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Fichajes.Add(new FichajeDTO
                {
                    IdFichaje = Convert.ToInt32(reader["IdFichaje"].ToString()),
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                    NombreUsuario = reader["NombreUsuario"].ToString(),
                    FechaHora = Convert.ToDateTime(reader["FechaHora"].ToString()),
                    TipoFichaje = (TipoFichaje)Convert.ToByte(reader["TipoFichaje"]),
                    Latitud = Convert.ToDouble(reader["Latitud"].ToString()),
                    Longitud = Convert.ToDouble(reader["Longitud"].ToString()),
                    Calle = reader["Calle"].ToString(),
                    Ciudad = reader["Ciudad"].ToString(),
                    FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString())

                });
            }

            return Fichajes;

        }

        public async Task<FichajeDTO> ObtenerPorIdFichajeAsync(int idfichaje)
        {
            using var comm = _connection.CreateConnection();
            using var cmd = comm.CreateCommand();

            cmd.CommandText = @"select f.IdFichaje, f.IdUsuario, u.NombreUsuario, f.FechaHora AT TIME ZONE 'UTC' AT TIME ZONE 'Romance Standard Time' AS FechaHora, f.TipoFichaje, f.Latitud, f.Longitud, f.Calle, f.Ciudad, f.FechaRegistro  FROM FICHAJES f INNER JOIN USUARIO U ON f.IdUsuario = u.IdUsuario WHERE f.IdFichaje = @IdFichaje";

            var param = cmd.CreateParameter();
            param.ParameterName = "@IdFichaje";
            param.Value = idfichaje;
            cmd.Parameters.Add(param);

            await comm.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ✅ CLAVE: si no hay fila, devuelve null (y el controller hará NotFound)
            if (!await reader.ReadAsync())
                return null;

            return new FichajeDTO
            {
                IdFichaje = reader["IdFichaje"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdFichaje"]),
                IdUsuario = reader["IdUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUsuario"]),
                NombreUsuario = reader["NombreUsuario"]?.ToString(),
                FechaHora = Convert.ToDateTime(reader["FechaHora"].ToString()),
                TipoFichaje = (TipoFichaje)Convert.ToByte(reader["TipoFichaje"]),
                Latitud = Convert.ToDouble(reader["Latitud"].ToString()),
                Longitud = Convert.ToDouble(reader["Longitud"].ToString()),
                Calle = reader["Calle"].ToString(),
                Ciudad = reader["Ciudad"].ToString(),
                FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString())    
            };
        }

        public async Task<IEnumerable<UsuarioDTO>> ListaUsuariosAsync()
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





    }
}

        /*
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
                */
   

