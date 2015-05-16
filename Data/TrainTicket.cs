using System;

namespace TravelAgency.Data
{
    class TrainTicket : Ticket
    {
        public TrainTicket(string from, string to, string departureDateTime, string priceString, string studentPriceString)
        {
            this.From = from; this.To = to;
            DateTime dateAndTime = ParseDateTime(departureDateTime);
            this.DateAndTime = dateAndTime; decimal price = decimal.Parse(priceString);
            this.Price = price;
            decimal studentPrice = decimal.Parse(studentPriceString);
            this.StudentPrice = studentPrice;
        }

        public TrainTicket(string from, string to, string departureDateTime)
        {
            this.From = from;
            this.To = to; DateTime dateAndTime = ParseDateTime(departureDateTime);
            this.DateAndTime = dateAndTime;
        }

        public decimal StudentPrice { get; set; }

        public override string Type
        {
            get
            {
                return "train";
            }
        }

        public override string UniqueKey
        {
            get
            {
                return this.Type + ";;" + this.From + ";" + this.To + ";" + this.DateAndTime + ";";
            }
        }
    }
}