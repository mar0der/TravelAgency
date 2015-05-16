using System.Globalization;

namespace TravelAgency
{
    using System;
    using Data;

    public class TravelAgencyMain
    {
        private static TicketCatalog ticketCatalog = new TicketCatalog();

        public static void Main()
        {

            while (true)
            {
                var commandString = Console.ReadLine();

                if (commandString == null)
                {
                    break;
                }

                commandString = commandString.Trim();
                var commandResult = ProcessCommand(commandString);

                if (commandResult != null)
                {
                    Console.WriteLine(commandResult);
                }
            }
        }

        public static string ProcessCommand(string line)
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
            string output = Constants.InvalidCommand;
            string[] parameters;

            switch (command)
            {
                case "AddAir":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessAddAirCommand(parameters);
                    break;
                case "DeleteAir":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessDeleteAirCommand(parameters);
                    break;
                case "AddTrain":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessAddTrainCommand(parameters);
                    break;

                case "DeleteTrain":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessDeleteTrainCommand(parameters);
                    break;

                case "AddBus":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessAddBusCommand(parameters);
                    break;

                case "DeleteBus":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessDeleteBusCommand(parameters);
                    break;

                case "FindTickets":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessFindTickets(parameters);
                    break;

                case "FindTicketsInInterval":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ProcessFindTicketsInInterval(parameters);
                    break;
            }

            return output;
        }

        private static string ProcessAddAirCommand(string[] parameters)
        {
            var flightNumber = parameters[0];
            var from = parameters[1];
            var to = parameters[2];
            var airline = parameters[3];
            var dateTime = ParseDateTime(parameters[4]);
            var price = decimal.Parse(parameters[5]);
            var result = ticketCatalog.AddAirTicket(flightNumber, from, to, airline, dateTime, price);
            return result;
        }

        private static string ProcessDeleteAirCommand(string[] parameters)
        {
            var flightNumber = parameters[0];
            var result = ticketCatalog.DeleteAirTicket(flightNumber);
            return result;
        }

        private static string ProcessAddTrainCommand(string[] parameters)
        {
            var from = parameters[0];
            var to = parameters[1];
            var departureDateTime = ParseDateTime(parameters[2]);
            var price = decimal.Parse(parameters[3]);
            decimal studentPrice = decimal.Parse(parameters[4]);
            var result = ticketCatalog.AddTrainTicket(from, to, departureDateTime, price, studentPrice);
            return result;
        }

        private static string ProcessDeleteTrainCommand(string[] parameters)
        {
            var from = parameters[0];
            var to = parameters[1];
            var departureDateTime = ParseDateTime(parameters[2]);
            var result = ticketCatalog.DeleteTrainTicket(from, to, departureDateTime);
            return result;
        }

        private static string ProcessAddBusCommand(string[] parameters)
        {
            var from = parameters[0];
            var to = parameters[1];
            var travelCompany = parameters[2];
            var departureDateTime = ParseDateTime(parameters[3]);
            var price = decimal.Parse(parameters[4]);
            var result = ticketCatalog.AddBusTicket(from, to, travelCompany, departureDateTime, price);
            return result;
        }

        private static string ProcessDeleteBusCommand(string[] parameters)
        {
            var from = parameters[0];
            var to = parameters[1];
            var travelCompany = parameters[2];
            var departureDateTime = ParseDateTime(parameters[3]);
            var result = ticketCatalog.DeleteBusTicket(from, to, travelCompany, departureDateTime);
            return result;
        }

        private static string ProcessFindTickets(string[] parameters)
        {
            var from = parameters[0];
            var to = parameters[1];
            var result = ticketCatalog.FindTickets(from, to);
            return result;
        }

        private static string ProcessFindTicketsInInterval(string[] parameters)
        {
            var fromDate = ParseDateTime(parameters[0]);
            var toDate = ParseDateTime(parameters[1]);
            var result = ticketCatalog.FindTicketsInInterval(fromDate, toDate);
            return result;
        }

        private static string[] ParseParameters(string line, int firstSpaceIndex)
        {
            var allParameters = line.Substring(firstSpaceIndex + 1);
            var parameters = allParameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].Trim();
            }
            return parameters;
        }

        private static DateTime ParseDateTime(string dateTimeStr)
        {
            var result = DateTime.ParseExact(
                dateTimeStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
            return result;
        }
    }
}