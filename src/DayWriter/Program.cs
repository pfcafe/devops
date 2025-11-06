
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;


class Program
{
    static void Main()
    {
        var connectionString = string.Empty;
        try
        {
            // Load configuration from environment variable
            connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Environment variable 'POSTGRES_CONNECTION_STRING' is not set.");
            }
        }
        catch (Exception ex)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            connectionString = config.GetConnectionString("PostgresConnection");
        }

        while (true)
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                // Migration to create date_log table
                var createDateLogTableQuery = @"CREATE TABLE IF NOT EXISTS date_log (id SERIAL PRIMARY KEY, log_time TIMESTAMP NOT NULL);";
                conn.Execute(createDateLogTableQuery);

                var now = DateTime.UtcNow;
                using var cmd = new NpgsqlCommand("INSERT INTO date_log (log_time) VALUES (@p)", conn);
                cmd.Parameters.AddWithValue("p", now);
                cmd.ExecuteNonQuery();

                Console.WriteLine($"Inserted current UTC time: {now}");

                Thread.Sleep(60000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
