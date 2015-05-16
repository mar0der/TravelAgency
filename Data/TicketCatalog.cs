using System.Linq;

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
        private Dictionary<string, Ticket> ticketsByUniqueKey = new Dictionary<string, Ticket>();
        private MultiDictionary<string, Ticket> ticketsByFromToDestination = new MultiDictionary<string, Ticket>(true);
        private OrderedMultiDictionary<DateTime, Ticket> ticketsByDepartionDateTime = new OrderedMultiDictionary<DateTime, Ticket>(true);

        public TicketCatalog()
        {

        }
        
        public string AddAirTicket(string flightNumber, string from, string to, string airline, DateTime dateTime, decimal price)
        {
            AirTicket ticket = new AirTicket(flightNumber, from, to, airline, dateTime, price);
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

            var result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.airTicketsCount--;
            }

            return result;
        }

        public string AddTrainTicket(string from, string to, DateTime departureDateTime, decimal price, decimal studentPrice)
        {
            var ticket = new TrainTicket(from, to, departureDateTime, price, studentPrice);
            var result = this.AddTicket(ticket);
            if (result.Contains("added"))
            {
                this.trainTicketsCount++;
            }

            return result;
        }

        public string DeleteTrainTicket(string from, string to, DateTime departureDateTime)
        {
            var ticket = new TrainTicket(from, to, departureDateTime);
            var result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.trainTicketsCount--;
            }

            return result;
        }

        public string AddBusTicket(string from, string to, string travelCompany, DateTime departureDateTime, decimal price)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime, price);
            string key = ticket.UniqueKey;
            string result;

            if (this.ticketsByUniqueKey.ContainsKey(key))
            {
                result = Constants.DuplicateTicket;
            }
            else
            {
                this.ticketsByUniqueKey.Add(key, ticket);
                var fromToKey = ticket.FromToKey;
                this.ticketsByFromToDestination.Add(fromToKey, ticket);
                this.ticketsByDepartionDateTime.Add(ticket.DateAndTime, ticket);
                result = Constants.TicketAdded;
            }

            if (result.Contains("added"))
            {
                this.busTicketsCount++;
            }
            return result;
        }

        public string DeleteBusTicket(string from, string to, string travelCompany, DateTime departureDateTime)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime);
            string result = this.DeleteTicket(ticket);

            if (result.Contains("deleted"))
            {
                this.busTicketsCount--;
            }

            return result;
        }
        
        public string FindTicketsInInterval(DateTime startDateTime, DateTime endDateTime)
        {
            var ticketsFound = this.ticketsByDepartionDateTime.Range(startDateTime, true, endDateTime, true).Values;
            if (ticketsFound.Count > 0)
            {
                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return Constants.NotFound;
        }

        public string FindTickets(string from, string to)
        {
            string fromToKey = Ticket.CreateFromToKey(from, to);
            if (this.ticketsByFromToDestination.ContainsKey(fromToKey))
            {
                List<Ticket> ticketsFound = this.ticketsByFromToDestination.Values.Where(t => t.FromToKey == fromToKey).ToList();

                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return Constants.NotFound;
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

        private string AddTicket(Ticket ticket)
        {
            var key = ticket.UniqueKey;

            if (this.ticketsByUniqueKey.ContainsKey(key))
            {
                return Constants.DuplicateTicket;
            }

            this.ticketsByUniqueKey.Add(key, ticket);
            string fromToKey = ticket.FromToKey;
            this.ticketsByFromToDestination.Add(fromToKey, ticket);
            this.ticketsByDepartionDateTime.Add(ticket.DateAndTime, ticket);
            return Constants.TicketAdded;
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

        private string DeleteTicket(Ticket ticket)
        {
            var key = ticket.UniqueKey;

            if (!this.ticketsByUniqueKey.ContainsKey(key))
            {
                return Constants.TicketNotExists;
            }

            ticket = this.ticketsByUniqueKey[key];
            this.ticketsByUniqueKey.Remove(key);
            string fromToKey = ticket.FromToKey;
            this.ticketsByFromToDestination.Remove(fromToKey, ticket);
            this.ticketsByDepartionDateTime.Remove(ticket.DateAndTime, ticket);
            return Constants.TicketDeleted;
        }
    }
}