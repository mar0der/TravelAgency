namespace TravelAgency.Data
{
    using System;

    public class AirTicket : Ticket
    {
        public AirTicket(string flightNumber, string from, string to, string airline, DateTime departureDateTime, decimal price)
        {
            this.FlightNumber = flightNumber;
            this.From = from;
            this.To = from;
            this.Company = airline;
            this.DateAndTime = departureDateTime;
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