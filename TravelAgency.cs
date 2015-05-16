namespace TravelAgency
{
    using System;
    using Data;

    public class TravelAgencyMain
    {
        public static void Main()
        {
            TicketCatalog ticketCatalog = new TicketCatalog();
            while (true)
            {
                var commandString = Console.ReadLine();

                if (commandString == null)
                {
                    break;
                }

                commandString = commandString.Trim();
                var commandResult = ticketCatalog.ProcessCommand(commandString);

                if (commandResult != null)
                {
                    Console.WriteLine(commandResult);
                }
            }
        }
    }
}