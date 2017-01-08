using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Bot.Services.States
{
    internal class ExchangeState:State
    {
        private const string text = @"Please enter the three-letter currency code in ISO 4217 format (eg USD).Use /return to exit";
        //{"Cur_ID":145,"Date":"2016-12-29T00:00:00","Cur_Abbreviation":"USD","Cur_Scale":1,"Cur_Name":"Доллар США","Cur_OfficialRate":1.9519}
        private const string RateApi = "http://www.nbrb.by/API/ExRates/Rates/{0}?ParamMode=2";
        private const string CurInfo = "http://www.nbrb.by/API/ExRates/Currencies/{0} ";
        internal ExchangeState(TelegramBotService botService, Update update) : base(botService, update) { }

        protected override async Task Handle()
        {
            var message = Update.Message;
            if (message != null && message.Text.Length == 3) {
                var obj = await ExchangeRate(message.Text);
                if (obj != null) {
                    var info = await GetCurrencyInfo(obj.CurrencyId);
                    if (info != null)
                    {
                        await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, $"{obj.CurScale} {info.Name} costs {obj.Rate} BYN");
                    }
                    else
                    {
                        await HandleError();
                    }
                } else {
                    await HandleError();
                }
            } else {
                await HandleError();
            }
        }
       
        protected override async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "You have entered an invalid currency format or currency not supported");
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, text);
        }

        private async Task<CurrencyInfo> GetCurrencyInfo(int id)
        {
            using (var client = new HttpClient())
            {
                var t = await client.GetAsync(string.Format(CurInfo, id.ToString()));
                if (t.IsSuccessStatusCode)
                {
                    var content = await t.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CurrencyInfo>(content);
                }
            }

            return null;
        }

        private async Task<ExchangeRateJson> ExchangeRate(string currency)
        {
            using (var client = new HttpClient()) {
                var t = await client.GetAsync(string.Format(RateApi, currency));
                if (t.IsSuccessStatusCode) {
                    var content = await t.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExchangeRateJson>(content);
                }
                return null;
            }
        }

        internal override StatesTypes StateTypesId { get; } = StatesTypes.Exchange;

        class ExchangeRateJson
        {
            [JsonProperty("Cur_OfficialRate", Required = Required.Always)]
            public string Rate { get; set; }
            [JsonProperty("Cur_Name", Required = Required.Always)]
            public string CurName { get; set; }
            [JsonProperty("Cur_Scale", Required = Required.Always)]
            public int CurScale { get; set; }
            [JsonProperty("Cur_ID", Required = Required.Always)]
            public int CurrencyId { get; set; }
        }

        class CurrencyInfo
        {
            [JsonProperty("Cur_Name_Eng", Required = Required.Always)]
            public string Name { get; set;}
        }
      
    }
}
