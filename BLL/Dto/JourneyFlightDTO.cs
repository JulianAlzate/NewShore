using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Dto
{
    public class JourneyFlightDTO
    {
        public int IdJourneyFlight { get; set; }
        public int IdFlight { get; set; }
        public int IdJourney { get; set; }

        public virtual FlightDTO IdFlightNavigation { get; set; }
        public virtual JourneyDTO IdJourneyNavigation { get; set; }
    }
}
