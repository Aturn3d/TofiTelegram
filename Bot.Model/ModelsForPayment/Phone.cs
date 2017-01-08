using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model.ModelsForPayment
{
    public class Phone
    {
        public Phone(string op, string number, decimal amount)
        {
            this.Operator = op;
            this.Number = number;
            this.Amount = amount;
        }

        public string Operator { get; }

        public string Number { get; }

        public decimal Amount { get; }
    }
}
