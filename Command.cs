using MySql.Data.MySqlClient;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnekBot
{
    public abstract class Command
    {
        public abstract string Name { get; set; }

        public abstract void Execute(Message message, TelegramBotClient client);

        public bool Contains(string name) => name == Name;
    }
}