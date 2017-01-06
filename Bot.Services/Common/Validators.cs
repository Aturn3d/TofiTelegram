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
    }
}
