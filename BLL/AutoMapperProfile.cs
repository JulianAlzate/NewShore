using API.Model;
using AutoMapper;
using BLL.Dto;
using BLL.Response;
using System.Collections.Generic;

namespace BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Journey, JourneyDTO>().ReverseMap();
            CreateMap<Flight, FlightDTO>().ReverseMap();
            CreateMap<Transport, TransportDTO>().ReverseMap();
            CreateMap<JourneyFlight, JourneyFlightDTO>().ReverseMap();

            CreateMap<FlightDTO, FlightResponse>().ReverseMap();
            CreateMap<TransportDTO, TransportResponse>().ReverseMap();
            CreateMap<JourneyDTO, JourneyResponse>().ReverseMap();
        }
    }
}
