using BLL.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Response
{
   public class JourneyResponse : TravelDTO
    {
        public List<FlightResponse> Flight { get; set; }
    }
}
