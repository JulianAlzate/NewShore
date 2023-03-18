using BLL.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Response
{
   public class FlightResponse
    {
        [JsonProperty("DepartureStation")]
        public string Origin { get; set; }
        [JsonProperty("ArrivalStation")]
        public string Destination { get; set; }
        public int Price { get; set; }

        public string FlightCarrier { get; set; } //Pendiente ocultar de la respuesta 
        public int FlightNumber { get; set; } //Pendiente ocultar de la respuesta 

        public TransportResponse Transport { get; set; }
    }
}
