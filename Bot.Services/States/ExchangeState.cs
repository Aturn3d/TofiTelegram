using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Newtonsoft.Json;

namespace Bot.Services.States
{
    internal class ExchangeState:State
    {
        //{"Cur_ID":145,"Date":"2016-12-29T00:00:00","Cur_Abbreviation":"USD","Cur_Scale":1,"Cur_Name":"Доллар США","Cur_OfficialRate":1.9519}
        private const string rateApi = "http://www.nbrb.by/API/ExRates/Rates/{0}?ParamMode=2";

        protected override async Task Handle()
        {
            var message = Update.Message;
            if (message != null && message.Text.Length == 3) {
                var obj = await ExchangeRate(message.Text);
                if (obj != null) {
                    await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, $"{obj.CurScale} {obj.CurName} стоит {obj.Rate} бел. рублей");
                } else {
                    await HandleError();
                }
            } else {
                await HandleError();
            }
        }

        protected override async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Вы ввели некоректный формат валюты");
            BotService.SetState(new InitialState());
        }


        private async Task<ExchangeRateJson> ExchangeRate(string currency)
        {
            using (var client = new HttpClient()) {
                var t = await client.GetAsync(string.Format(rateApi, currency));
                if (t.IsSuccessStatusCode) {
                    var content = await t.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExchangeRateJson>(content);
                }
                return null;
            }
        }

        public override StatesTypes StateTypesId { get; } = StatesTypes.Exchange;

        class ExchangeRateJson
        {
            [JsonProperty("Cur_OfficialRate", Required = Required.Always)]
            public string Rate { get; set; }
            [JsonProperty("Cur_Name", Required = Required.Always)]
            public string CurName { get; set; }
            [JsonProperty("Cur_Scale", Required = Required.Always)]
            public int CurScale { get; set; }
        }
    }
}
