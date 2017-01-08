using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model.ModelsForPayment
{
    public class Ticket
    {
        public Ticket(string placeFrom, string placeTo, DateTime departureDate, decimal amount)
        {
            this.PlaceFrom = placeFrom;
            this.PlaceTo = placeTo;
            this.DepartureDate = departureDate;
            this.Amount = amount;
        }

        public string PlaceFrom { get; }

        public string PlaceTo { get; }

        public DateTime DepartureDate { get; }

        public decimal Amount { get; }

        public static Ticket GetTicket(string code)
        {
            if (!string.IsNullOrEmpty(code) && code.Length == 4)
            {
                return new Ticket("Minsk", "Brest", DateTime.Now.AddDays(2), 51);
            }

            return null;
        }

        public override string ToString()
        {
            return $"Place from :{PlaceFrom}\n Place to:{PlaceTo} \n Departure date {DepartureDate.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
