using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using Serilog;

namespace HoltronBot
{
    public class DatabaseAccess
    {
        private const string DB_CONNECTION_STRING = "Data Source=SporkData.db";

        public DatabaseAccess()
        {
            Initialize();
        }

        public static SqliteConnection GetDBConnection()
        {
            var conn = new SqliteConnection(DB_CONNECTION_STRING);
            conn.Open();
            return conn;
        }

        private static bool Initialize()
        {
            using var conn = GetDBConnection();
            try
            {
                var command = conn.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS appliedScripts (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error("Failed to create appliedScripts table. Error: {Message}", ex.Message);
                //Console.WriteLine($"Failed to create appliedScripts table. Error: {ex.Message}");
                return false;
            }


            return RunSQLFiles(conn);
        }

        private static bool RunSQLFiles(SqliteConnection conn)
        {
            var query = conn.CreateCommand();
            query.CommandText = "SELECT name FROM appliedScripts;";
            var previouslyRunScripts = new List<string>();
            try
            {
                using var reader = query.ExecuteReader();
                while (reader.Read())
                {
                    previouslyRunScripts.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to pull previously applied scripts. Error: {Message}", ex.Message);
                //Console.WriteLine($"Failed to pull previously applied scripts. Error: {ex.Message}");
                return false;
            }

            try
            {
                var files = Directory.GetFiles(".\\SQLScripts");
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    Log.Information("Found SQL Script {fileName}", fileName);
                    //Console.WriteLine($"Found SQL Script {fileName}");
                    if (!previouslyRunScripts.Contains(fileName))
                    {
                        Log.Information("Applying SQL Script {fileName}", fileName);
                        //Console.WriteLine($"Applying SQL Script {fileName}");
                        var command = conn.CreateCommand();
                        command.CommandText = File.ReadAllText(file);
                        command.ExecuteNonQuery();
                        command.CommandText = $"INSERT INTO appliedScripts (name) VALUES ('{fileName}');";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to apply new scripts. Error: {Message}", ex.Message);
                //Console.WriteLine($"Failed to apply new scripts. Error: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}