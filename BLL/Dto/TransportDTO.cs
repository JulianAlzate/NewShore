using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Dto
{
   public class TransportDTO
    {
        public TransportDTO()
        {
            Flights = new HashSet<FlightDTO>();
        }

        public int IdTransport { get; set; }
        public string FlightCarrier { get; set; }
        public int FlightNumber { get; set; }

        public virtual ICollection<FlightDTO> Flights { get; set; }
    }
}
