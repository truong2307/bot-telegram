using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegrambot.Module;

namespace Telegrambot
{
    public class Handlers
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient
           , Update update
           , CancellationToken cancellationToken)
        {
            //command list

            switch (true)
            {
                case true when update.Message.Text == "!bit":
                    await botClient.ReplyMessage(update, cancellationToken);
                    break;
                default:
                    break;
            }
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
