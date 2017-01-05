using System.Collections.Generic;

namespace Bot.Model
{
    public class User
    {
        public User()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string NickName { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public int ChatState { get; set; }
        public string MobilePhone { get; set; }

        public CreditCard CreditCard { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
