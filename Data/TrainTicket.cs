namespace TravelAgency.Data
{
    using System;

    public class TrainTicket : Ticket
    {
        public TrainTicket(string from, string to, DateTime departureDateTime, decimal price, decimal studentPrice)
        {
            this.From = from; 
            this.To = to;
            this.DateAndTime = departureDateTime; 
            this.Price = price;
            this.StudentPrice = studentPrice;
        }

        public TrainTicket(string from, string to, DateTime departureDateTime)
        {
            this.From = from;
            this.To = to; 
            this.DateAndTime = departureDateTime;
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