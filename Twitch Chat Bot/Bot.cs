using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Chat_Bot
{
    public class Bot
    {

        string ip = "irc.chat.twitch.tv";
        int port = 6667;
        string password = "oauth:"; // your twitch bot account oauth code
        string botUsername = ""; // your bot username


        public async Task ConnectBot()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);

            var streamReader = new StreamReader(tcpClient.GetStream());
            var streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };



            await streamWriter.WriteLineAsync($"PASS {password}");
            await streamWriter.WriteLineAsync($"NICK {botUsername}");
            await streamWriter.WriteLineAsync($"JOIN #yourchannelnamehere");
            //  await streamWriter.WriteLineAsync($"PRIVMSG #theblack0wl :Hey I just started my IRC bot");

            while (true)
            {
                string line = await streamReader.ReadLineAsync();
                Console.WriteLine(line);

                string[] split = line.Split(" ");
                //PING :tmi.twitch.tv
                //Respond with PONG :tmi.twitch.tv

                if (split.Length > 1 && split[1] == "PRIVMSG")
                {
                    //:mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv 
                    // ^^^^^^^^
                    //Grab this name here
                    int exclamationPointPosition = split[0].IndexOf("!");
                    string username = split[0].Substring(1, exclamationPointPosition - 1);
                    //Skip the first character, the first colon, then find the next colon
                    int secondColonPosition = line.IndexOf(':', 1);//the 1 here is what skips the first character
                    string message = line.Substring(secondColonPosition + 1);//Everything past the second colon
                    Console.WriteLine($"{username} said '{message}'");

                    if (message.Equals("!PING"))
                    {
                        await SendMessageAsync(streamWriter,"PONG");
                    }
                }
            }

        }

        private static async Task SendMessageAsync(StreamWriter writer,string message)
        {
            await writer.WriteLineAsync($"PRIVMSG #yourchannelnamehere :{message}");
        }

    }
}
