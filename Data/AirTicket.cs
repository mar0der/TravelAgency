namespace TravelAgency.Data
{
    using System;
    class AirTicket : Ticket
    {
        public AirTicket(string flightNumber, string from, string to, string airline,
            string departureDateTime, string stringPrice)
        {
            this.FlightNumber = flightNumber;
            this.From = from;
            this.To = from;
            this.Company = airline;
            DateTime dateAndTime = ParseDateTime(departureDateTime);
            this.DateAndTime = dateAndTime;
            decimal price = decimal.Parse(stringPrice);
            this.Price = price;
        }

        public string FlightNumber { get; set; }

        public AirTicket(string flightNumber)
        {
            this.FlightNumber = flightNumber;
        }

        public override string Type
        {
            get
            {
                return "air";
            }
        }
        public override string UniqueKey
        {
            get
            {
                return this.Type + ";;" + this.FlightNumber;
            }
        }
    }
}