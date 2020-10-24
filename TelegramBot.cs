using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace AnekBot
{
    public class TelegramBot
    {
        private TelegramBotClient _bot;
        private List<Command> _commands;
        private MySqlConnection _connection;
        
        public TelegramBot()
        {
            var way = "";
            foreach (var dir in Directory.GetCurrentDirectory().Split('\\').SkipLast(4))
                way += dir + '\\';
            var reader = new StreamReader(way + @"properties.txt");
            var token = "";
            long moderId = 0;
            var serverInfo = new string[4];
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(' ');
                switch (line[0])
                {
                    case "token":
                        token = line[1];
                        break;
                    case "server":
                        serverInfo[0] = line[1];
                        break;
                    case "userId":
                        serverInfo[1] = line[1];
                        break;
                    case "password":
                        serverInfo[2] = line[1];
                        break;
                    case "database":
                        serverInfo[3] = line[1];
                        break;;
                    case "moderId":
                        moderId = long.Parse(line[1]);
                        break;
                    default:
                            throw new Exception("wrong properties " + line[0]);
                }
            }
            _bot = new TelegramBotClient(token);
            _bot.SetWebhookAsync("");
            _bot.OnMessage += BotOnOnMessage;
            _bot.OnCallbackQuery += BotOnOnCallbackQuery;
            
            _commands = new List<Command>()
            {
                new GetAnekCommand(),
                new AddAnekCommand(moderId)
                //new CloseCommand(moderId)
            };
            _connection = new MySqlConnection($"server={serverInfo[0]};" +
                                              $"user={serverInfo[1]};" +
                                              $"database={serverInfo[3]};" +
                                              $"password={serverInfo[2]}");
            Console.WriteLine("Do you create tables Users and Anekdots");
            bool isCreated = bool.Parse(Console.ReadLine());
            UsersBase.CreateBase(_connection, isCreated);
            AnekBase.CreateBase(_connection, isCreated);
            
            _bot.StartReceiving();
        }

        private void BotOnOnCallbackQuery(object? sender, CallbackQueryEventArgs e)
        {
            if(e.CallbackQuery.Data == "Yes")
                AnekBase.AddAnek(e.CallbackQuery.Message.Text);
            _bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
        }

        private void BotOnOnMessage(object? sender, MessageEventArgs e)
        {
            var message = e.Message;
            foreach (var command in _commands)
                if (message.Text.StartsWith(command.Name))
                    command.Execute(message, _bot);
        }
    }
}