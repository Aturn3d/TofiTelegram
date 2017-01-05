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
