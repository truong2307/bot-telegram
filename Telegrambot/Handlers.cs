using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using static System.Collections.Specialized.BitVector32;
using Microsoft.VisualBasic;

namespace Telegrambot
{
    public class Handlers
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient
           , Update update
           , CancellationToken cancellationToken)
        {
            //if (update.Type != UpdateType.Message)
            //    return;
            //// Only process text messages
            //if (update.Message!.Type != MessageType.Text)
            //    return;

            //command bit
            if (update.Message.Text == "!bit")
            {
                await PriceBtc(botClient, update, cancellationToken);
            }
        }

        private static async Task PriceBtc(ITelegramBotClient botClient
            , Update update
            , CancellationToken cancellationToken)
        {
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

        public static Task HandleErrorAsync(ITelegramBotClient botClient
            , Exception exception
            , CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
