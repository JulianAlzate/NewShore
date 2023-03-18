using System;
using System.Collections.Generic;

#nullable disable

namespace API.Model
{
    public partial class Flight
    {
        public Flight()
        {
            JourneyFlights = new HashSet<JourneyFlight>();
        }

        public int IdFlight { get; set; }
        public int? IdTransport { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal? Price { get; set; }
        public virtual Transport IdTransportNavigation { get; set; }
        public virtual ICollection<JourneyFlight> JourneyFlights { get; set; }
    }
}
