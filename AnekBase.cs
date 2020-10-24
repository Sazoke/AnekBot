using System;
using MySql.Data.MySqlClient;

namespace AnekBot
{
    public static class AnekBase
    {
        private static MySqlConnection _connection;
        
        public static void CreateBase(MySqlConnection connection, bool isCreated)
        {
            _connection = connection;
            if(isCreated)
                return;
            _connection.Open();
            var sql = @"CREATE TABLE Users(
                            Id PRIMARY KEY auto_increment,
                            TextOfAnek mediumtext)";
            var cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        public static string GetAnek(int AnekId)
        {
            _connection.Open();
            var sql = $"SELECT TextOfAnek FROM Anekdots WHERE Id = {AnekId}";
            var cmd = new MySqlCommand(sql, _connection);
            var reader = cmd.ExecuteReader();
            var result = "Анеков Больше нет";
            if (reader.Read())
                result = reader[0].ToString();
            _connection.Close();
            
            return result;
        }

        public static void AddAnek(string anek)
        {
            _connection.Open();
            var sql = "INSERT INTO Anekdots (TextOfAnek) VALUE (@anek)";
            var cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.Add("@anek", MySqlDbType.MediumText).Value = anek;
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }
}