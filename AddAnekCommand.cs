using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnekBot
{
    public class AddAnekCommand : Command
    {
        private long _idOfModer;
        public AddAnekCommand(long idOfModer)
        {
            _idOfModer = idOfModer;
        }
        private Command _commandImplementation;
        public override string Name { get; set; } = "/add";

        public override void Execute(Message message, TelegramBotClient client)
        {
            if (message.Text.Length <= 5)
            {
                client.SendTextMessageAsync(message.Chat.Id, "Вы не ввели анекдот после add");
                return;
            }

            var anek = message.Text.Substring(5);
            
            var buttonYes = new InlineKeyboardButton();
            buttonYes.Text = "Yes";
            buttonYes.CallbackData = "Yes";
            
            var buttonNo = new InlineKeyboardButton();
            buttonNo.Text = "No";
            buttonNo.CallbackData = "No";
            
            var keyBoard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new []
                    {
                        buttonYes, buttonNo
                    }
                }
            );
            client.SendTextMessageAsync(_idOfModer, 
                anek, 
                ParseMode.Default, 
                false, 
                false, 
                0, 
                keyBoard);
        }
    }
}