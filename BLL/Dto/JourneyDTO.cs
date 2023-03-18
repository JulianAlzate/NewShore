using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Dto
{
    public class JourneyDTO : TravelDTO
    {
        public JourneyDTO()
        {
            JourneyFlights = new HashSet<JourneyFlightDTO>();
        }

        public int IdJourney { get; set; }


        public virtual ICollection<JourneyFlightDTO> JourneyFlights { get; set; }

    }
}
