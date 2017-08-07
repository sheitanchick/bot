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
            Bot.OnMessage += DoSomeStaff;
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

        public static void DoSomeStaff(object sender, MessageEventArgs arg)
        {
            var messageq = arg.Message;
            var key = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
                new InlineKeyboardCallbackButton("hi", "by")
            }
        );
            Bot.SendTextMessageAsync(messageq.Chat.Id, "Darov", replyMarkup: key);
            Console.WriteLine(messageq.From.FirstName);
            Console.WriteLine(sender.ToString());
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            var keyboard = new ReplyKeyboardMarkup(
                new KeyboardButton[]
                {
                    new KeyboardButton("Bang!!!"),
                    new KeyboardButton("Dang!!!")
                }
                );

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("ку"))
            {
                //await Task.Delay(500); // simulate longer running task
                await Bot.SendTextMessageAsync(message.Chat.Id, "<b>Не кукай мне тут</b>", replyMarkup: keyboard, parseMode: ParseMode.Html);

            }
            FileToSend file = new FileToSend(new Uri("http://bm.img.com.ua/nxs/img/prikol/images/large/0/0/307600.jpg"));
            if (message.Text == "Bang!!!")
                await Bot.SendPhotoAsync(message.Chat.Id, file);
            if (message.From.LastName == "")
                await Bot.SendTextMessageAsync(message.Chat.Id, "Тебя, не спрашивали");
            if (message.Text == "Dang!!!")
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var mes = callbackQueryEventArgs.CallbackQuery.Message;
            if (callbackQueryEventArgs.CallbackQuery.Data == "by") Bot.SendTextMessageAsync(mes.Chat.Id, "yo");
            //Bot.SendTextMessageAsync(mes.Chat.Id, "Dratuti");
            Console.WriteLine(callbackQueryEventArgs.CallbackQuery);
        }
    }
}