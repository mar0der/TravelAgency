using System;

namespace TravelAgency.Data
{
    class BusTicket : Ticket
    {
        public BusTicket(string from, string to, string travelCompany, string departureDateTime, string priceString)
        {
            this.From = from;
            this.To = to; this.Company = travelCompany;
            DateTime dateAndTime = ParseDateTime(departureDateTime);

            this.DateAndTime = dateAndTime;
            decimal price = decimal.Parse(priceString);
            this.Price = price;
        }

        public BusTicket(string from, string to, string travelCompany, string departureDateTime)
        {
            this.From = from;
            this.To = to; this.Company = travelCompany;
            DateTime dateAndTime = ParseDateTime(departureDateTime);
            this.DateAndTime = dateAndTime;
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