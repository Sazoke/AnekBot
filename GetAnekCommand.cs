using MySql.Data.MySqlClient;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnekBot
{
    public class GetAnekCommand : Command
    {
        public override string Name { get; set; } = "/get";
        public override void Execute(Message message, TelegramBotClient client)
        {
            var user = UsersBase.GetUser(message.From.Id);
            var anekId = user.LastAnekId + 1;
            var anek = AnekBase.GetAnek(anekId);
            client.SendTextMessageAsync(message.Chat.Id, anek);
            if(anek != "Анеков Больше нет")
                UsersBase.SetNewLastAnek(user.Id,anekId);
        }
    }
}