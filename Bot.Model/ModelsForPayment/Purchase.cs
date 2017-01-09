using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model.ModelsForPayment
{
    public class Purchase
    {
        public Purchase(string name, decimal amount, int count)
        {
            this.Name = name;
            this.Amount = amount;
            this.Count = count;
        }

        public string Name { get; }

        public decimal Amount { get; }

        public int Count { get; }

        public static Purchase GetPurchase(string code)
        {
            if (!string.IsNullOrEmpty(code) && code.Length == 4)
            {
                return new Purchase("Kettle", 33, 2);
            }

            return null;
        }

        public override string ToString()
        {
            return $"Purchase name : {Name};\nUnit price : {Amount.ToString()}$;\nPurchase count: {Count.ToString()};";
        }
    }
}
