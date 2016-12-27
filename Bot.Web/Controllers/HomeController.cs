using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bot.Model;
using Bot.Services;

namespace Bot.Web.Controllers
{
    public class HomeController : ApiController
    {
        private IUserService userService;


        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpGet]
        public IEnumerable<User> Get()
        {
            var u = userService.GetUsers();
            userService.CreateUser(new User()
            {
                NickName = "Жора"
            });
            return  userService.GetUsers(); 
        }


    }
}
