namespace TravelAgency.Interfaces
{
    using System;
    using Enumerations;
    /// <summary>
    /// Defines ticket catalog for (air, bus and train tickets). Sets methods for
    /// addin, deleating and finding tickets
    /// </summary>
    public interface ITicketCatalog
    {
        /// <summary>
        /// Adds air ticket to the catalog by given flight number from and to location airline datetime and price
        /// </summary>
        /// <param name="flightNumber">Unique flight number</param>
        /// <param name="from">Departure location</param>
        /// <param name="to">Arriving location</param>
        /// <param name="airline">Airline name</param>
        /// <param name="dateTime">Departure date time</param>
        /// <param name="price">Price</param>
        /// <returns>As a result the command prints “Ticket added” or “Duplicate ticket” if such flight already exists.</returns>
        string AddAirTicket(string flightNumber, string from, string to, string airline, DateTime dateTime, decimal price);

        string DeleteAirTicket(string flightNumber);

        string AddTrainTicket(string from, string to, DateTime dateTime, decimal price, decimal studentPrice);

        string DeleteTrainTicket(string from, string to, DateTime dateTime);

        string AddBusTicket(string from, string to, string travelCompany, DateTime dateTime, decimal price);

        /// <summary>
        /// Adds bus ticket to the catalog by given from and to location, travel company and departure datetime
        /// </summary>
        /// <param name="from">Departure location</param>
        /// <param name="to">Arrival location</param>
        /// <param name="travelCompany">Travel company name</param>
        /// <param name="dateTime">Departure date time</param>
        /// <returns>Returns "Ticket deleted" if ticket is found and "Ticket does not exist" if tickets is not found</returns>
        string DeleteBusTicket(string from, string to, string travelCompany, DateTime dateTime);


        /// <summary>
        /// Finds all tickets by departure and arival location
        /// </summary>
        /// <param name="from">Departure location</param>
        /// <param name="to">Arival location</param>
        /// <returns>finds all tickets from the catalog by given departure town/airport (from) and arrival town/airport (to). As a result the 
        /// command prints all matching tickets on a single line, separated by spaces, in format [date and time; type; price] where type is either
        ///  “air” or “bus” or “train” ordered by date and time (as first criteria, ascending), then by type (as second criteria, ascending) and then 
        /// by price (as third criteria, ascending). In case of train tickets the regular price is printed and the student’s price is disregarded. 
        /// Prices are always printed with exactly 2 digits after the decimal point. If no tickets are found by the specified criteria, the command prints “Not found”.
        /// </returns>
        string FindTickets(string from, string to);

        /// <summary>
        /// Finds all tickets between two dates
        /// </summary>
        /// <param name="startDateTime">Start date</param>
        /// <param name="endDateTime">End Date</param>
        /// <returns>Finds all tickets between two date from the catalog. As a result the command
        ///  prints all matching tickets on a single line, separated by spaces, in format [date and time; type; price] where type is either “air” or “bus” 
        /// or “train” ordered by date and time (as first criteria, ascending), then by type (as second criteria, ascending) and then by price (as third criteria, 
        /// ascending). In case of train tickets the regular price is printed and the student’s price is disregarded. Prices are always printed with exactly 2 digits 
        /// after the decimal point. If no tickets are found by the specified criteria, the command prints “Not found”.
        /// </returns>
        string FindTicketsInInterval(DateTime startDateTime, DateTime endDateTime);

        int GetTicketsCount(TicketType ticketType);
    }
}