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

        public string ProcessCommand(string line)
        {
            if (line == string.Empty)
            {
                return null;
            }

            int firstSpaceIndex = line.IndexOf(' ');

            if (firstSpaceIndex == -1)
            {
                return Constants.InvalidCommand;
            }

            string command = line.Substring(0, firstSpaceIndex);
            string output = "Invalid command!";

            switch (command)
            {
                case "AddAir":
                    string allParameters = line.Substring(firstSpaceIndex + 1);
                    string[] parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.AddAirTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                    break;
                case "DeleteAir":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.DeleteAirTicket(parameters[0]);
                    break;
                case "AddTrain":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.AddTrainTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                    break;

                case "DeleteTrain":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.DeleteTrainTicket(parameters[0], parameters[1], parameters[2]);
                    break;

                case "AddBus": allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.AddBussTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                    break;

                case "DeleteBus":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.DeleteBusTicket(parameters[0], parameters[1], parameters[2], parameters[3]);
                    break;

                case "FindTickets":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.FindTickets(parameters[0], parameters[1]);
                    break;

                case "FindTicketsInInterval":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = this.FindTicketsInInterval(parameters[0], parameters[1]);
                    break;
            }

            return output;
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

        public string AddDeleteTicket(Ticket ticket, bool isAdd)
        {
            if (isAdd)
            {
                string key = ticket.UniqueKey;
                if (this.Dict.ContainsKey(key))
                {
                    return "Duplicate ticket";
                }
                else
                {
                    this.Dict.Add(key, ticket);
                    string fromToKey = ticket.FromToKey;

                    this.Dict2.Add(fromToKey, ticket);
                    this.Dict3.Add(ticket.DateAndTime, ticket);
                    return "Ticket added";
                }
            }
            else
            {
                string key = ticket.UniqueKey;

                if (this.Dict.ContainsKey(key))
                {
                    ticket = this.Dict[key];
                    this.Dict.Remove(key);
                    string fromToKey = ticket.FromToKey;
                    this.Dict2.Remove(fromToKey, ticket);
                    this.Dict3.Remove(ticket.DateAndTime, ticket);
                    return "Ticket deleted";
                }

                return "Ticket does not exist";
            }
        }

        public string AddAirTicket(string flightNumber, string from, string to, string airline, string departureDateTime, string price)
        {
            AirTicket ticket = new AirTicket(flightNumber, from, to, airline, departureDateTime, price);

            string result = this.AddDeleteTicket(ticket, true);

            if (result.Contains("added"))
            {
                this.airTicketsCount++;
            }

            return result;
        }

        protected string DeleteAirTicket(string flightNumber)
        {
            AirTicket ticket = new AirTicket(flightNumber);

            string result = this.AddDeleteTicket(ticket, false);

            if (result.Contains("deleted"))
            {
                this.airTicketsCount--;
            }

            return result;
        }

        public string AddTrainTicket(string from, string to, string departureDateTime, string price, string studentPrice)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime, price, studentPrice);
            string result = this.AddDeleteTicket(ticket, true);
            if (result.Contains("added"))
            {
                this.trainTicketsCount++;
            }

            return result;
        }

        public string DeleteTrainTicket(string from, string to, string departureDateTime)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime);
            string result = this.AddDeleteTicket(ticket, false);

            if (result.Contains("deleted"))
            {
                this.trainTicketsCount--;
            }

            return result;
        }

        protected string AddBussTicket(string from, string to, string travelCompany, string departureDateTime, string price)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime, price);
            string key = ticket.UniqueKey;
            string result;

            if (this.Dict.ContainsKey(key))
            {
                result = "Duplicate ticket";
            }
            else
            {
                this.Dict.Add(key, ticket);
                string fromToKey = ticket.FromToKey;
                this.Dict2.Add(fromToKey, ticket);
                this.Dict3.Add(ticket.DateAndTime, ticket);
                result = "Ticket added";
            }

            if (result.Contains("added"))
            {
                this.busTicketsCount++;
            }

            return result;
        }

        private string DeleteBusTicket(string from, string to, string travelCompany, string departureDateTime)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime);
            string result = this.AddDeleteTicket(ticket, false);

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

            return "Not found";
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

            return "Not found";
        }

        public string FindTicketsInInterval(DateTime startDateTime, DateTime endDateTime)
        {
            var ticketsFound = this.Dict3.Range(startDateTime, true, endDateTime, true).Values;
            if (ticketsFound.Count > 0)
            {
                string ticketsAsString = ReadTickets(ticketsFound);
                return ticketsAsString;
            }

            return "Not found";
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