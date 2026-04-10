using Microsoft.Data.SqlClient;

namespace mifichaje.Infraestructura.Database
{
    public abstract class BaseRepository
    {
        private readonly DBConnectionFactory _connection;

        protected BaseRepository(DBConnectionFactory connection)
        {
            _connection = connection;
        }

        protected async Task<SqlConnection> OpenConnectionAsync()
        {
            var conn = _connection.CreateConnection();

            int retries = 3;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    await conn.OpenAsync();
                    return conn;
                }
                catch (SqlException ex) when (i < retries - 1)
                {
                    await Task.Delay(1000 * (i + 1)); // backoff
                }
            }

            throw new Exception("Error conectando a Azure SQL tras varios intentos");
        }
    }
}