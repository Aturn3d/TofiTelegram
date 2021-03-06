﻿using System;
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
        Payment TransferMoney(User user);
        Payment PhonePay(User user);
        Payment InternetPay(User user);
    }
    public class PaymentService: IPaymentService
    {
        //TODO: Use DI?
        static AuthPayment pay = new AuthPayment();
        public Payment TransferMoney(User user)
        {
            var card = new Card(user.CurrentPayment.To);
            return AuthPayment.TransferMoney(user, card, user.CurrentPayment.Amount);
        }

        public Payment PhonePay(User user)
        {
            var phone = new Phone(user.CurrentPayment.To, user.CurrentPayment.Account, user.CurrentPayment.Amount);
            return AuthPayment.PhonePay(user, phone);
        }

        public Payment InternetPay(User user)
        {
            var internet = new Internet(user.CurrentPayment.To, user.CurrentPayment.Account, user.CurrentPayment.Amount);
            return AuthPayment.InternetPay(user, internet);
        }

        public Payment TicketPay(User user)
        {
            var ticket = Ticket.GetTicket(user.CurrentPayment.Account);
            return AuthPayment.TicketPay(user, ticket);
        }  

        public Payment MakePurchase(User user)
        {
            var purchase = Purchase.GetPurchase(user.CurrentPayment.Account);
            return AuthPayment.MakePurchase(user, purchase);
        }
    }
}
