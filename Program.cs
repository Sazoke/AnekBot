using System;
using System.Threading.Tasks;

namespace AnekBot
{
    class Program
    {
        private static TelegramBot _bot;
        
        static void Main(string[] args)
        {
            MainTask().GetAwaiter().GetResult();
        }

        public static async Task MainTask()
        {
            _bot = new TelegramBot();
            await Task.Delay(-1);
        }
    }
}