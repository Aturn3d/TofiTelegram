using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;

namespace Bot.Services.States.Commands
{
   internal class LastPaymentsCommand:ICommand
   {
       private const string Format = "\nDetails:{0}\nDate:{1}\nState:{2}\n";
       public async Task Execute(TelegramBotService botService)
       {
           var payments = botService.User.Payments.OrderByDescending(d => d.Date).Take(10);
           var text = new StringBuilder();
           foreach (var p in payments) {
               text.AppendFormat(Format, p.TransactionDescription, p.Date.Date.ToShortDateString(), p.Description);
           }
           var mess = text.Length == 0 ? "No payments yet" : text.ToString();
           await botService.Bot.SendTextMessageAsync(botService.User.ChatId, mess);
           botService.SetState(new InitialState(botService, null));
       }
    }
}
