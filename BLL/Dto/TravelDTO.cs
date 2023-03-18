using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Dto
{
    public class TravelDTO
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal? Price { get; set; }
    }
}
