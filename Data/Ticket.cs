namespace TravelAgency.Data
{
    using System;
    using System.Globalization;

    public abstract class Ticket : IComparable<Ticket>
    {
        public abstract string Type { get; }

        public virtual string From { get; set; }

        public virtual string To { get; set; }

        public virtual string Company { get; set; }

        public virtual DateTime DateAndTime { get; set; }

        public virtual decimal Price { get; set; }

        public virtual decimal SpecialPrice { get; set; }

        public abstract string UniqueKey { get; }

        public override string ToString()
        {
            string input = "[" + this.DateAndTime.ToString("dd.MM.yyyy HH:mm") + "; " + this.Type + "; " + string.Format("{0:f2}", this.Price) + "]";
            return input;
        }

        public string FromToKey
        {
            get
            {
                return CreateFromToKey(this.From, this.To);
            }
        }

        public static string CreateFromToKey(string from, string to)
        {
            return from + "; " + to;
        }

        public static DateTime ParseDateTime(string departureDateTime)
        {
            var result = DateTime.ParseExact(departureDateTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            return result;
        }

        public int CompareTo(Ticket otherTicket)
        {
            int result = this.DateAndTime.CompareTo(otherTicket.DateAndTime); 

            if (result == 0)
            {
                result = this.Type.CompareTo(otherTicket.Type);
            } 

            ////TODO see this logic
            if (result == 0)
            {
                result = this.Price.CompareTo(otherTicket.Price);
            }

            return result;
        }
    }
}