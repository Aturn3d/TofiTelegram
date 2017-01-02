using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Bot.Services;
using Telegram.Bot.Types;
using User = Bot.Model.User;
using TelegramBot;

namespace Bot.Web.Controllers
{
    public class HomeController : ApiController
    {
        private ITelegramBotService botService;

        public HomeController(ITelegramBotService botService)
        {
            this.botService = botService;
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]Update value)
        {
            await botService.HandleUpdate(value);
            return Ok();
        }

    }
}
