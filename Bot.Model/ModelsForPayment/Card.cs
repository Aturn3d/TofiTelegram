using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model.ModelsForPayment
{
    public class Card
    {
        public Card(string number)
        {
            Number = number;
        }
        public string Number { get;  }
    }
}
