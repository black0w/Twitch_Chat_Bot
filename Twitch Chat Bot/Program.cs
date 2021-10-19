using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Twitch_Chat_Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bot bot = new Bot();
            bot.ConnectBot();

            await Task.Delay(-1);
        }
    }
}
