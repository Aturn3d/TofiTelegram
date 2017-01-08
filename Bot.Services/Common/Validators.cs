﻿using Bot.Model.ModelsForPayment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.Common
{
    public class Validators
    {
        private static HashSet<string> AvailableCardNumbers = new HashSet<string>
        {
            "4007000000027",
            "4012888818888",
            "4111111111111111"
        };

        static Dictionary<string, Func<string, bool>> InternetProvidersValidator = new Dictionary<string, Func<string, bool>>() 
        { 
            {
                "ByFly", s => 
                {
                    ulong d;
                    return s.Length == 13 && ulong.TryParse(s, out d);
                }
            },
            {
                "CosmosTv", s =>
                 {
                    ulong d;
                    return s.Length == 7 && ulong.TryParse(s, out d);
                 }
            },
            {
                "Adsl", s =>
                 {
                    ulong d;
                    return s.Length == 10 && ulong.TryParse(s, out d);
                 }
            }
        };

public static bool ValidateCardNumber(string cardNumber)
        {
            //For test pursope card number should be only equal to test card
            return AvailableCardNumbers.Contains(cardNumber);
        }

        public static bool ValidateCvvCode(string cvv)
        {
            int n;
            //For test pursope cvv length should be equal to 3 and be numeric
            return cvv.Length == 3 && int.TryParse(cvv, out n);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expirationDate">Date in format MMyy</param>
        /// <returns></returns>
        public static bool IsExpirationCardDateValid(string expirationDate)
        {
            DateTime expDate;
            var parsed = DateTime.TryParseExact(expirationDate, "MMyy",CultureInfo.CurrentCulture,DateTimeStyles.None,  out expDate);
            return parsed && DateTime.Now.Date < expDate.Date;
        }

        public static List<string> ValidateInternet(Internet internet)
        {
            var errors = new List<string>();
            Func<string, bool> v;
            var haveProvider = InternetProvidersValidator.TryGetValue(internet.Provider, out v);
            if (haveProvider)
            {
                var correctAccount = v(internet.Account);
                if (!correctAccount)
                {
                    errors.Add("Your account number isn't valid");
                }
            }
            else
            {
                errors.Add("We don't support this provider");
            }

            if (internet.Amount < 1 || internet.Amount > 100)
            {
                errors.Add("amount should be from 1 to 100 dollars");
            }

            return errors;
        }
    }
}
