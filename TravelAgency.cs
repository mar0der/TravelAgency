using System;
using TravelAgency.Data;

namespace TravelAgency
{
    class TravelAgencyMain
    {
        static void Main()
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