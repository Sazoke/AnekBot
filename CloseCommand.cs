using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnekBot
{
    public class CloseCommand : Command
    {
        private long _moderId;

        public CloseCommand(long moderId)
        {
            _moderId = moderId;
        }
        
        public override string Name { get; set; } = "/close";
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (message.Chat.Id != _moderId)
                return;
            client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            client.StopReceiving();
            Environment.Exit(0);
        }
    }
}