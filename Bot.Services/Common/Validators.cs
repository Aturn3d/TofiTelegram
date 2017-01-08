using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Bot.Model.ModelsForPayment;

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

        private static readonly Dictionary<string, Func<string, bool>> InternetProvidersValidator = new Dictionary
            <string, Func<string, bool>>
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


        private static readonly Dictionary<string, string> PhoneValidator = new Dictionary<string, string>
        {
            {
                "Mts", @"\+375(29|33)[0-9]{7}\b"
            },
            {
                "Velcom", @"\+375(29|44)[0-9]{7}\b"
            },
            {
                "Life", @"\+375(29|25)[0-9]{7}\b"
            }
        };

        public static bool ValidateCardNumber(string cardNumber)
        {
            //For test pursope card number should be only equal to test card
            //return AvailableCardNumbers.Contains(cardNumber);
            return true;
        }

        public static bool ValidateCvvCode(string cvv)
        {
            int n;
            //For test pursope cvv length should be equal to 3 and be numeric
            return cvv.Length == 3 && int.TryParse(cvv, out n);
        }

        /// <summary>
        /// </summary>
        /// <param name="expirationDate">Date in format MMyy</param>
        /// <returns></returns>
        public static bool IsExpirationCardDateValid(string expirationDate)
        {
            DateTime expDate;
            var parsed = DateTime.TryParseExact(expirationDate, "MMyy", CultureInfo.CurrentCulture, DateTimeStyles.None,
                out expDate);
            return parsed && DateTime.Now.Date < expDate.Date;
        }

        public static List<string> ValidateInternet(Internet internet)
        {
            var errors = new List<string>();
            Func<string, bool> v;
            var haveProvider = InternetProvidersValidator.TryGetValue(internet.Provider, out v);
            if (haveProvider) {
                var correctAccount = v(internet.Account);
                if (!correctAccount) {
                    errors.Add("Your account number isn't valid");
                }
            }
            else {
                errors.Add("We don't support this provider");
            }

            if (internet.Amount < 1 || internet.Amount > 100) {
                errors.Add("amount should be from 1 to 100 dollars");
            }

            return errors;
        }

        public static List<string> ValidatePhone(Phone phone)
        {
            var errors = new List<string>();
            string v;
            var haveProvider = PhoneValidator.TryGetValue(phone.Operator, out v);
            if (haveProvider) {
                var correctNumber = new Regex(v).IsMatch(phone.Number);
                if (!correctNumber) {
                    errors.Add("Uncorrect number for this operator");
                }
            }
            else {
                errors.Add("We don't support this operator");
            }

            if (phone.Amount < 1 || phone.Amount > 100) {
                errors.Add("amount should be from 1 to 100 dollars");
            }

            return errors;
        }
    }
}
