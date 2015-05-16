namespace TravelAgencyUnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TravelAgency.Data;

    [TestClass]
    public class TicketCatalogTests
    {
        private TicketCatalog ticketCatalog = new TicketCatalog();

        [ClassInitialize]
        public void ClassInit()
        {
            var airTicket1 = new AirTicket("asdfasf");
            var airTicket2 = new AirTicket("sdgsdg");
            var busTicket1 = new BusTicket("sof", "vie", "union ivkoni", "27.01.2015 21:20", "12");
            var busTicket2 = new BusTicket("sof", "plovdiv", "group", "27.01.2015 21:20", "120");
            var trainTicket1 = new TrainTicket("sof", "varna", "27.01.2015 21:20", "12", "6");
            var trainTicket2 = new TrainTicket("sof", "burgas", "28.01.2015 21:20", "12", "6");
        }

        [TestMethod]
        public void AddAirTicket_ShouldReturnCorrectResult()
        {
           // this.ticketCatalog.AddAirTicket()
        }
    }
}
