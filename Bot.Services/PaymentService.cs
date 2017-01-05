using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizePayment;
using Bot.Model;
using Bot.Model.ModelsForPayment;

namespace Bot.Services
{
    public interface IPaymentService
    {
        PaymentsResponse TransferMoney(User user, string cardNumberTo, decimal amount);
    }
    public class PaymentService: IPaymentService
    {
        //TODO: Use DI?
        static AuthPayment pay = new AuthPayment();
        public PaymentsResponse TransferMoney(User user, string cardNumberTo, decimal amount)
        {
            var card = new Card(cardNumberTo);
            return AuthPayment.TranferMoney(user, card, amount);
        }
    }
}
