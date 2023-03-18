using System;
using System.Collections.Generic;

#nullable disable

namespace API.Model
{
    public partial class Journey
    {
        public Journey()
        {
            JourneyFlights = new HashSet<JourneyFlight>();
        }

        public int IdJourney { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<JourneyFlight> JourneyFlights { get; set; }
    }
}
