using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class CurrentPaymentInfo
    {
        public int Id { get; set; }
        //CardNumber/Provider(tel and internet)/
        public string To { get; set; }
        //Mobile number/ Inet number / Ticker number/ 
        public string Account { get; set; }

        public decimal Amount { get; set; }
    }
}
