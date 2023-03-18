using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Dto
{
    public class FlightDTO : TravelDTO
    {
        public FlightDTO()
        {
            JourneyFlights = new HashSet<JourneyFlightDTO>();
        }

        public int IdFlight { get; set; }
        public int IdTransport { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }

        protected virtual TransportDTO IdTransportNavigation { get; set; }
        protected virtual ICollection<JourneyFlightDTO> JourneyFlights { get; set; }
    }
}
