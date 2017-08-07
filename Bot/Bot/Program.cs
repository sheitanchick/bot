using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.Echo
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("436441359:AAE-7PmtkSzoOCzwfDJsEZDVNxq4YaXo97o");

        static void Main(string[] args)
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            //Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }
        

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            var keyboard = new ReplyKeyboardMarkup(
                new KeyboardButton[]
                {
                    new KeyboardButton("Bitch!!!"),
                    new KeyboardButton("Fuck!!!")
                }
                );

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("ку")) 
            {
                //await Task.Delay(500); // simulate longer running task
                await Bot.SendTextMessageAsync(message.Chat.Id, "Не кукай мне тут", replyMarkup: keyboard);
                
            }
            FileToSend file = new FileToSend(new Uri("http://bm.img.com.ua/nxs/img/prikol/images/large/0/0/307600.jpg"));
            if (message.Text == "Bitch!!!")
                await Bot.SendPhotoAsync(message.Chat.Id, file);
            if (message.From.LastName == "Nekrash")
                await Bot.SendTextMessageAsync(message.Chat.Id, "Тебя, бля не спрашивали ска");
            if (message.Text == "Fuck!!!")
                await Bot.SendTextMessageAsync(message.Chat.Id, "fuck yoou", replyMarkup: null);
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Console.WriteLine(callbackQueryEventArgs.CallbackQuery);
        }
    }
}
