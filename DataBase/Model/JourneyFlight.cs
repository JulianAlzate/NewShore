using System;
using System.Collections.Generic;

#nullable disable

namespace API.Model
{
    public partial class JourneyFlight
    {
        public int IdJourneyFlight { get; set; }
        public int IdFlight { get; set; }
        public int IdJourney { get; set; }

        public virtual Flight IdFlightNavigation { get; set; }
        public virtual Journey IdJourneyNavigation { get; set; }
    }
}
