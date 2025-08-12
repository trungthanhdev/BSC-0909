using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BSC_0909.Domain.Services.Bot
{
    public class BotModelService
    {
        // private static TelegramBotClient botClient;
        private readonly string chatID = Environment.GetEnvironmentVariable("CHAT_ID") ?? throw new InvalidOperationException("ChatID not found!");
        private readonly string BotClientID = Environment.GetEnvironmentVariable("BOT_CLIENT_ID") ?? throw new InvalidOperationException("BotClientId not found!");
        // public BotModelService()
        // {
        //     botClient = new TelegramBotClient(BotClientID);
        // }
        public void SendMessageToTelegram(string message)
        {
            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            urlString = String.Format(urlString, BotClientID, chatID, message);
            WebRequest request = WebRequest.Create(urlString);

            Stream rs = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(rs);
            string? line = "";
            StringBuilder sb = new StringBuilder();
            while (line != null)
            {
                line = reader.ReadLine();
                if (line != null)
                    sb.Append(line);
            }
            string response = sb.ToString();
            // System.Console.WriteLine($"Response: {response}");
            // System.Console.WriteLine("Gui tin nhan thanh cong");
        }
    }
}
