namespace TravelAgency.Data
{
    using System;

    public class BusTicket : Ticket
    {
        public BusTicket(string from, string to, string travelCompany, DateTime departureDateTime, decimal price)
        {
            this.From = from;
            this.To = to; 
            this.Company = travelCompany;
            this.DateAndTime = departureDateTime;
            this.Price = price;
        }

        public BusTicket(string from, string to, string travelCompany, DateTime departureDateTime)
        {
            this.From = from;
            this.To = to; 
            this.Company = travelCompany;
            this.DateAndTime = departureDateTime;
        }

        public override string Type
        {
            get
            {
                return "bus";
            }
        }

        public override string UniqueKey
        {
            get
            {
                return this.Type + ";;" + this.From + ";" + this.To + ";" +
                       this.Company + this.DateAndTime + ";";
            }
        }
    }
}