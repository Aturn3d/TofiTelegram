using System.Threading.Tasks;
using System.Web.Http;
using Bot.Services;
using Telegram.Bot.Types;

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
