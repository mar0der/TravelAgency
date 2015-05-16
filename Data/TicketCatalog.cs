namespace TravelAgency.Data
{
    using System;
    using System.Collections.Generic;
    using Enumerations;
    using Interfaces;
    using Wintellect.PowerCollections;

    public class TicketCatalog : ITicketCatalog
    {
        private int airTicketsCount = 0;
        private int busTicketsCount = 0;
        private int trainTicketsCount = 0;
        private Dictionary<string, Ticket> Dict = new Dictionary<string, Ticket>();
        private MultiDictionary<string, Ticket> Dict2 = new MultiDictionary<string, Ticket>(true);
        private OrderedMultiDictionary<DateTime, Ticket> Dict3 = new OrderedMultiDictionary<DateTime, Ticket>(true);

        public TicketCatalog()
        {

        }

        public int GetTicketsCount(string type)
        {
            if (type == "air")
            {
                return this.airTicketsCount;
            }

            if (type == "bus")
            {
                return this.busTicketsCount;
            }

            return this.trainTicketsCount;
        }

        public string AddTicket(Ticket ticket)
        {
            var key = ticket.UniqueKey;

            if (this.Dict.ContainsKey(key))
            {
                return Constants.DuplicateTicket;
            }

            this.Dict.Add(key, ticket);
            string fromToKey = ticket.FromToKey;
            this.Dict2.Add(fromToKey, ticket);
            this.Dict3.Add(ticket.DateAndTime, ticket);
            return Constants.TicketAdded;
        }

        public string DeleteTicket(Ticket ticket)
        {
            var key = ticket.UniqueKey;

            if (!this.Dict.ContainsKey(key))
            {
                return Constants.TicketNotExists;
            }

            ticket = this.Dict[key];
            this.Dict.Remove(key);
            string fromToKey = ticket.FromToKey;
            this.Dict2.Remove(fromToKey, ticket);
            this.Dict3.Remove(ticket.DateAndTime, ticket);
            return Constants.TicketDeleted;
        }

        public string AddAirTicket(string flightNumber, string from, string to, string airline, string departureDateTime, string price)
        {
            AirTicket ticket = new AirTicket(flightNumber, from, to, airline, departureDateTime, price);

            string result = this.AddTicket(ticket);

            if (result.Contains("added"))
            {
                this.airTicketsCount++;
            }

            return result;
        }

        public string DeleteAirTicket(string flightNumber)
        {
            AirTicket ticket = new AirTicket(flightNumber);

            string result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.airTicketsCount--;
            }

            return result;
        }

        public string AddTrainTicket(string from, string to, string departureDateTime, string price, string studentPrice)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime, price, studentPrice);
            string result = this.AddTicket(ticket);
            if (result.Contains("added"))
            {
                this.trainTicketsCount++;
            }

            return result;
        }

        public string DeleteTrainTicket(string from, string to, string departureDateTime)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime);
            string result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.trainTicketsCount--;
            }

            return result;
        }

        public string AddBussTicket(string from, string to, string travelCompany, string departureDateTime, string price)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime, price);
            string key = ticket.UniqueKey;
            string result;

            if (this.Dict.ContainsKey(key))
            {
                result = Constants.DuplicateTicket;
            }
            else
            {
                this.Dict.Add(key, ticket);
                string fromToKey = ticket.FromToKey;
                this.Dict2.Add(fromToKey, ticket);
                this.Dict3.Add(ticket.DateAndTime, ticket);
                result = Constants.TicketAdded;
            }

            if (result.Contains("added"))
            {
                this.busTicketsCount++;
            }

            return result;
        }

        public string DeleteBusTicket(string from, string to, string travelCompany, string departureDateTime)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime);
            string result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.busTicketsCount--;
            }

            return result;
        }

        private static string ReadTickets(ICollection<Ticket> tickets)
        {
            List<Ticket> sortedTickets = new List<Ticket>(tickets);
            sortedTickets.Sort();
            string result = string.Empty;

            for (int i = 0; i < sortedTickets.Count; i++)
            {
                Ticket ticket = sortedTickets[i];
                result += ticket.ToString();
                if (i < sortedTickets.Count - 1)
                {
                    result += " ";
                }
            }

            return result;
        }

        public string FindTickets(string from, string to)
        {
            string fromToKey = Ticket.CreateFromToKey(from, to);
            if (this.Dict2.ContainsKey(fromToKey))
            {
                List<Ticket> ticketsFound = new List<Ticket>();
                foreach (var t in this.Dict2.Values)
                {
                    if (t.FromToKey == fromToKey)
                    {
                        ticketsFound.Add(t);
                    }
                }

                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return Constants.NotFound;
        }

        public string FindTicketsInInterval(string startDateTimeStr, string endDateTimeStr)
        {
            DateTime startDateTime = Ticket.ParseDateTime(startDateTimeStr);
            DateTime endDateTime = Ticket.ParseDateTime(endDateTimeStr);
            string ticketsAsString = this.FindTicketsInInterval(startDateTime, endDateTime);

            return ticketsAsString;
        }

        public string FindTicketsInInterval2(DateTime startDateTime, DateTime endDateTime)
        {
            var ticketsFound = this.Dict3.Range(startDateTime, true, endDateTime, true).Values;

            if (ticketsFound.Count > 0)
            {
                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return Constants.NotFound;
        }

        public string FindTicketsInInterval(DateTime startDateTime, DateTime endDateTime)
        {
            var ticketsFound = this.Dict3.Range(startDateTime, true, endDateTime, true).Values;
            if (ticketsFound.Count > 0)
            {
                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return Constants.NotFound;
        }

        public string AddAirTicket(string flightNumber, string from, string to, string airline, DateTime dateTime, decimal price)
        {
            var result = this.AddAirTicket(flightNumber, from, to, airline, dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString());
            return result;
        }

        string ITicketCatalog.DeleteAirTicket(string flightNumber)
        {
            var result = this.DeleteAirTicket(flightNumber);
            return result;
        }

        public string AddTrainTicket(string from, string to, DateTime dateTime, decimal price, decimal studentPrice)
        {
            var result = this.AddTrainTicket(from, to, dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString(), studentPrice.ToString());
            return result;
        }

        public string DeleteTrainTicket(string from, string to, DateTime dateTime)
        {
            var result = this.DeleteTrainTicket(from, to, dateTime.ToString("dd.MM.yyyy HH:mm"));
            return result;
        }

        public string AddBusTicket(string from, string to, string travelCompany, DateTime dateTime, decimal price)
        {
            var result = this.AddBussTicket(from, to, travelCompany, dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString());
            return result;
        }

        public string DeleteBusTicket(string from, string to, string travelCompany, DateTime dateTime)
        {
            var result = this.DeleteBusTicket(from, to, travelCompany, dateTime.ToString("dd.MM.yyyy HH:mm"));
            return result;
        }

        public int GetTicketsCount(TicketType ticketType)
        {
            if (ticketType == TicketType.Air)
            {
                return this.airTicketsCount;
            }

            if (ticketType == TicketType.Bus)
            {
                return this.busTicketsCount;
            }

            return this.trainTicketsCount;
        }
    }
}