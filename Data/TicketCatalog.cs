using System;
using System.Collections.Generic;
using TravelAgency.Enumerations;
using TravelAgency.Interfaces;
using Wintellect.PowerCollections;

namespace TravelAgency.Data
{
    public class TicketCatalog : ITicketCatalog
    {
        internal Dictionary<string, Ticket> Dict = new Dictionary<string, Ticket>();
        MultiDictionary<string, Ticket> Dict2 = new MultiDictionary<string, Ticket>(true);
        internal OrderedMultiDictionary<DateTime, Ticket> Dict3 = new OrderedMultiDictionary<DateTime, Ticket>(true);

        public int airTicketsCount = 0;
        public int busTicketsCount = 0;
        public int trainTicketsCount = 0;
        public int GetTicketsCount(string type)
        {
            if (type == "air")
            {
                return airTicketsCount;
            }
            if (type == "bus")
            {
                return busTicketsCount;
            }
            return trainTicketsCount;
        }

        internal string AddDeleteTicket(Ticket ticket, bool isAdd)
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

            string result = AddDeleteTicket(ticket, true);
            if (result.Contains("added"))
            {
                airTicketsCount++;
            }
            return result;
        }

        protected string DeleteAirTicket(string flightNumber)
        {
            AirTicket ticket = new AirTicket(flightNumber);

            string result = AddDeleteTicket(ticket, false);

            if (result.Contains("deleted"))
            {
                airTicketsCount--;
            }
            return result;
        }

        public string AddTrainTicket(string from, string to, string departureDateTime, string price, string studentPrice)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime, price, studentPrice);
            string result = this.AddDeleteTicket(ticket, true);
            if (result.Contains("added"))
            {
                trainTicketsCount++;
            }
            return result;
        }

        string DeleteTrainTicket(string from, string to, string departureDateTime)
        {
            TrainTicket ticket = new TrainTicket(from, to, departureDateTime);
            string result = AddDeleteTicket(ticket, false);

            if (result.Contains("deleted"))
            {
                trainTicketsCount--;
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
                busTicketsCount++;
            }
            return result;
        }

        private string DeleteBusTicket(string from, string to, string travelCompany, string departureDateTime)
        {
            BusTicket ticket = new BusTicket(from, to, travelCompany, departureDateTime);
            string result = AddDeleteTicket(ticket, false);

            if (result.Contains("deleted"))
            {
                busTicketsCount--;
            }
            return result;
        }

        internal static string ReadTickets(ICollection<Ticket> tickets)
        {
            List<Ticket> sortedTickets = new List<Ticket>(tickets);
            sortedTickets.Sort();
            string result = "";

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

        public string findTicketsInInterval(string startDateTimeStr, string endDateTimeStr)
        {
            DateTime startDateTime = Ticket.ParseDateTime(startDateTimeStr);
            DateTime endDateTime = Ticket.ParseDateTime(endDateTimeStr);
            string ticketsAsString = FindTicketsInInterval(startDateTime, endDateTime);

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

        internal string ProcessCommand(string line)
        {
            if (line == "")
            {
                return null;
            }

            int firstSpaceIndex = line.IndexOf(' ');

            if (firstSpaceIndex == -1)
            {
                return "Invalid command!";
            }

            string command = line.Substring(0, firstSpaceIndex);
            string output = "Invalid command!";

            switch (command)
            {
                case "AddAir":
                    string allParameters = line.Substring(firstSpaceIndex + 1);
                    string[] parameters = allParameters.Split(
                        new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }
                    output = AddAirTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                    break;
                case "DeleteAir":
                    allParameters = line.Substring(firstSpaceIndex + 1); 
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = DeleteAirTicket(parameters[0]);
                    break;
                case "AddTrain":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = AddTrainTicket(parameters[0], parameters[1], parameters[2],
                        parameters[3], parameters[4]);
                    break;

                case "DeleteTrain":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    } output = DeleteTrainTicket(parameters[0], parameters[1], parameters[2]); 
                    break;

                case "AddBus": allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }
                    output = AddBussTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                    break;

                case "DeleteBus":
                    allParameters = line.Substring(firstSpaceIndex + 1); 
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = DeleteBusTicket(parameters[0], parameters[1], parameters[2], parameters[3]);
                    break;

                case "FindTickets":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = FindTickets(parameters[0], parameters[1]);
                    break;

                case "FindTicketsInInterval":
                    allParameters = line.Substring(firstSpaceIndex + 1);
                    parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Trim();
                    }

                    output = findTicketsInInterval(parameters[0], parameters[1]);
                    break;
            }
            return output;
        }

        public string AddAirTicket(string flightNumber, string from, string to, string airline, DateTime dateTime, decimal price)
        {
            return AddAirTicket(flightNumber, from, to, airline,


                dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString());
        }

        string ITicketCatalog.DeleteAirTicket(string flightNumber)
        {
            return DeleteAirTicket(flightNumber);
        }

        public string AddTrainTicket(string from, string to, DateTime dateTime, decimal price, decimal studentPrice)
        {
            var output = AddTrainTicket(from, to, dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString(), studentPrice.ToString());
            return output;
        }

        public string DeleteTrainTicket(string from, string to, DateTime dateTime)
        {
            var output =  DeleteTrainTicket(from, to, dateTime.ToString("dd.MM.yyyy HH:mm"));
            return output;
        }

        public string AddBusTicket(string from, string to, string Sayahat_ki_Tanzeem, DateTime dateTime, decimal price)
        {
            var output = AddBussTicket(from, to, Sayahat_ki_Tanzeem, dateTime.ToString("dd.MM.yyyy HH:mm"), price.ToString());
            return output;
        }

        public string DeleteBusTicket(string from, string to, string travelCompany, DateTime dateTime)
        {
            var output = DeleteBusTicket(from, to, travelCompany, dateTime.ToString("dd.MM.yyyy HH:mm"));
            return output;
        }

        public int GetTicketsCount(TicketType ticketType)
        {
            if (ticketType == TicketType.Air)
            {
                return airTicketsCount;
            }

            if (ticketType == TicketType.Bus)
            {
                return busTicketsCount;
            }

            return trainTicketsCount;
        }
    }
}