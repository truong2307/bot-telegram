using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace Telegrambot
{
    class Program
    {
        private static TelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient(Configuration.BotToken);

            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };

            using var cts = new CancellationTokenSource();

            botClient.StartReceiving(
                    Handlers.HandleUpdateAsync,
                    Handlers.HandleErrorAsync,
                           receiverOptions,
                           cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
    }
}
