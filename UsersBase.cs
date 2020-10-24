using System;
using MySql.Data.MySqlClient;

namespace AnekBot
{
    public static class UsersBase
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
                            TelegramId long,
                            LastAnekId int)";
            var cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
        
        public static User GetUser(int TelegramId)
        {
            if(_connection is null)
                throw new Exception();
            
            _connection.Open();
            var sql = $"SELECT * FROM Users WHERE TelegramId = {TelegramId}";
            var command = new MySqlCommand(sql, _connection);
            
            var reader = command.ExecuteReader();
            User result = null;
            if (!reader.Read())
            {
                _connection.Close();
                result = AddNewUser(TelegramId);
            }

            if(result is null)
                result = new User(){Id = (int)reader[0], TelegramId = TelegramId, LastAnekId = (int)reader[2]};
            
            _connection.Close();
            return result;
        }

        private static User AddNewUser(int TelegramId)
        {
            _connection.Open();
            var sql = "INSERT INTO Users(TelegramId, LastAnekId) value (@TelegramId, @LastAnekId)";
            var cmd = new MySqlCommand(sql, _connection);
            
            var telegramParam = cmd.Parameters.Add("@TelegramId", MySqlDbType.Int16);
            telegramParam.Value = TelegramId;

            var lastAnekParam = cmd.Parameters.Add("@LastAnekId", MySqlDbType.Int16);
            lastAnekParam.Value = 0;

            var id = cmd.ExecuteNonQuery();
            _connection.Close();
            return new User(){Id = id, LastAnekId = 0, TelegramId = TelegramId};
        }

        public static void SetNewLastAnek(int id, int newLastAnekID)
        {
            _connection.Open();
            var newSql = "UPDATE Users SET LastAnekId = @newAnek WHERE Id = @id";
            var cmd = new MySqlCommand(newSql,_connection);
            cmd.Parameters.Add("@newAnek", MySqlDbType.Int32).Value = newLastAnekID;
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            cmd.ExecuteNonQuery();
            _connection.Close();
        }
        
        
    }
}