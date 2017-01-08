using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model.ModelsForPayment
{
    public class Internet
    {
        public Internet(string provider, string account, decimal amount)
        {
            this.Provider = provider;
            this.Account = account;
            this.Amount = amount;
        }

        public string Provider { get; }

        public string Account { get; }

        public decimal Amount { get; }
    }
}
