using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegrambot.Module
{
    public static class CommandHandler
    {
        public static async Task ReplyMessage (this ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var client = new HttpClient();
            var result = await client.GetStringAsync("https://api.blockchain.com/v3/exchange/l2/BTC-USD");

            var objects = JObject.Parse(result);

            var post = objects["bids"][0];

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            await botClient.SendTextMessageAsync(
                chatId,
                text: "BTC to USD " + "\n" + "1 BTC = " + post["px"] + " $".ToString(),
                replyToMessageId: update.Message.MessageId,
                cancellationToken: cancellationToken);
        }

    }
}
