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
                    output = ticketCatalog.AddAirTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                    break;
                case "DeleteAir":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.DeleteAirTicket(parameters[0]);
                    break;
                case "AddTrain":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.AddTrainTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                    break;

                case "DeleteTrain":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.DeleteTrainTicket(parameters[0], parameters[1], parameters[2]);
                    break;

                case "AddBus": 
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.AddBussTicket(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                    break;

                case "DeleteBus":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.DeleteBusTicket(parameters[0], parameters[1], parameters[2], parameters[3]);
                    break;

                case "FindTickets":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.FindTickets(parameters[0], parameters[1]);
                    break;

                case "FindTicketsInInterval":
                    parameters = ParseParameters(line, firstSpaceIndex);
                    output = ticketCatalog.FindTicketsInInterval(parameters[0], parameters[1]);
                    break;
            }

            return output;
        }

        private static string[] ParseParameters(string line, int firstSpaceIndex)
        {
            var allParameters = line.Substring(firstSpaceIndex + 1);
            var parameters = allParameters.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].Trim();
            }
            return parameters;
        }
    }
}