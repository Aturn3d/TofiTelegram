using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string TransactionDescription { get; set; }

    }
}
