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
                string line = Console.ReadLine();

                if (line == null)
                {
                    break;
                }

                line = line.Trim();
                string commandResult = ticketCatalog.ProcessCommand(line);

                if (commandResult != null)
                {
                    Console.WriteLine(commandResult);
                }
            }
        }
    }
}