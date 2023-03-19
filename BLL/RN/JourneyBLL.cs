using API.Model;
using AutoMapper;
using BLL.Common;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Response;
using DataBase.DbManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using static BLL.Common.Enums;

namespace BLL.RN
{
    public class JourneyBLL : IBusinessLogic<JourneyDTO>, IJourney
    {
        private readonly IMapper _mapper;
        public Repositorio<Journey> _repo;
        private readonly IBusinessLogic<FlightDTO> _flightBLL;
        private readonly IBusinessLogic<TransportDTO> _transportBLL;
        private readonly IBusinessLogic<JourneyFlightDTO> _journeyFlightBLL;

        public JourneyBLL(IMapper mapper, IBusinessLogic<FlightDTO> flightBLL,
            IBusinessLogic<TransportDTO> transportBLL,
            IBusinessLogic<JourneyFlightDTO> journeyFlightBLL)
        {
            _mapper = mapper;
            _repo = new Repositorio<Journey>(new NewShoreContext());
            _flightBLL = flightBLL;
            _transportBLL = transportBLL;
            _journeyFlightBLL = journeyFlightBLL;
        }

        public JourneyDTO BuscarPorId(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int Crear(JourneyDTO model)
        {
            JourneyDTO existDB = ListAll().FirstOrDefault(x => x.Origin == model.Origin && x.Destination == model.Destination);
            if (existDB == null)
            {
                Journey modelDB = _mapper.Map<Journey>(model);
                _repo.Crear(modelDB);
                _repo.Confirmar();
                return modelDB.IdJourney;
            }
            return existDB.IdJourney;
        }

        public bool Existe(string origin, string destination)
        {
            return ListAll().Any(x => x.Origin == origin && x.Destination == destination);
        }

        public IList<JourneyDTO> ListAll()
        {
            List<Journey> modelBD = _repo.Listar.ToList();
            List<JourneyDTO> modelDTO = _mapper.Map<List<JourneyDTO>>(modelBD);
            return modelDTO;
        }
        public List<JourneyResponse> ObtenerVuelos(RequestFilter requestFilter)
        {
            List<JourneyResponse> responses = BuscarTrayecto(requestFilter);//1 Consultar en BD la existencia
            if (responses == null || responses.Count == 0)
            {
                responses = ObtenerVuelosApi(requestFilter);
                if (responses != null && responses.Count > 0)
                {
                    //Almacenar en BD
                    GuargarTodo(responses);
                }
            }
            return responses;
        }

        private List<JourneyResponse> ObtenerVuelosApi(RequestFilter requestFilter)
        {
            List<JourneyResponse> journeyUser = new List<JourneyResponse>();
            TipoVuelo codApiVuelo = TipoVuelo.Unico;
            bool conEscala = false;
            if (requestFilter.withReturn)
            {
                codApiVuelo = TipoVuelo.Retorno;
                if (requestFilter.withMultiple)//Se permiten vuelos de ida y regreso con escala
                {
                    conEscala = true;
                }
            }
            else if (requestFilter.withMultiple)
            {
                codApiVuelo = TipoVuelo.Multiple;
            }

            List<FlightResponse> flights = ((IFlight)_flightBLL).ConsultarTodosLosVuelos(codApiVuelo);
            if (flights != null)
            {
                journeyUser = ObtenerRuta(requestFilter, flights, codApiVuelo, conEscala);
            }
            return journeyUser;
        }

        private List<JourneyResponse> ObtenerRuta(RequestFilter requestFilter, List<FlightResponse> flights, TipoVuelo codApiVuelo, bool conEscala)
        {
            List<JourneyResponse> journeyUser = new List<JourneyResponse>();
            if (codApiVuelo == TipoVuelo.Retorno)
            {
                if (conEscala)
                {

                }
                else
                {
                    List<FlightResponse> vueloIda = flights
                        .Where(x => x.Origin == requestFilter.origin && x.Destination == requestFilter.destination)
                        .Select(x => new FlightResponse()
                        {
                            Destination = x.Destination,
                            Origin = x.Origin,
                            Price = x.Price,
                            Transport = new TransportResponse()
                            {
                                FlightCarrier = x.FlightCarrier,
                                FlightNumber = x.FlightNumber,
                            },
                        }).ToList();
                    if (vueloIda != null)
                    {
                        List<FlightResponse> vueloRegreso = flights
                            .Where(x => x.Origin == requestFilter.destination &&
                                        x.Destination == requestFilter.origin)
                             .Select(x => new FlightResponse()
                             {
                                 Destination = x.Destination,
                                 Origin = x.Origin,
                                 Price = x.Price,
                                 Transport = new TransportResponse()
                                 {
                                     FlightCarrier = x.FlightCarrier,
                                     FlightNumber = x.FlightNumber,
                                 },
                             }).ToList();
                        if (vueloRegreso != null) //No cumple con las condiciones del usuario
                        {
                            List<FlightResponse> flightsUser = vueloIda.Concat(vueloRegreso).ToList();//Revisar el orden
                            decimal priceTotal = flightsUser.Sum(x => x.Price); //Segun la info de la api no van haber dos vuelos con el mismo origen yd estino, de ser asi cambiaria la logica
                            journeyUser.Add(new JourneyResponse()
                            {
                                Destination = requestFilter.destination,
                                Origin = requestFilter.origin,
                                Price = priceTotal,
                                Flight = flightsUser
                            });
                        }
                    }

                }
            }
            else if (codApiVuelo == TipoVuelo.Multiple)
            {
            }
            else
            {
                FlightResponse flightsUser = flights.FirstOrDefault(x => x.Origin == requestFilter.origin && x.Destination == requestFilter.destination);
                if (flightsUser != null)
                {
                    List<FlightResponse> flight = new List<FlightResponse>();
                    flightsUser.Transport = new TransportResponse()
                    {
                        FlightCarrier = flightsUser.FlightCarrier,
                        FlightNumber = flightsUser.FlightNumber
                    };
                    flight.Add(flightsUser);
                    journeyUser.Add(new JourneyResponse()
                    {
                        Destination = flightsUser.Destination,
                        Origin = flightsUser.Origin,
                        Price = flightsUser.Price,
                        Flight = flight
                    });
                }
            }
            return journeyUser;
        }

        private List<JourneyResponse> BuscarTrayecto(RequestFilter model)
        {
            List<JourneyResponse> journeyRresponse = new List<JourneyResponse>();
            List<JourneyDTO> Journey = BuscarTrayectoBD(model.origin, model.destination);
            if (Journey != null && Journey.Count > 0)
            {
                
                foreach (JourneyDTO item in Journey)
                {
                    JourneyResponse journeyModel = new JourneyResponse();
                    journeyModel.Flight = new List<FlightResponse>();
                    journeyModel.Origin = item.Origin;
                    journeyModel.Destination = item.Destination;
                    journeyModel.Price = item.Price;
                    item.JourneyFlights = ((IJourneyFlight)_journeyFlightBLL).ListAll().Where(x=>x.IdJourney == item.IdJourney).ToList();

                    foreach (JourneyFlightDTO itemFligth in item.JourneyFlights)
                    {
                        FlightDTO modelDTO = ((IFlight)_flightBLL).BuscarPorId(itemFligth.IdFlight);
                        FlightResponse modelResponse = _mapper.Map<FlightResponse>(modelDTO);

                        TransportDTO transportDTO = ((ITransport)_transportBLL).BuscarPorId(modelDTO.IdTransport);
                        TransportResponse transportResponse = _mapper.Map<TransportResponse>(transportDTO);
                        modelResponse.Transport = transportResponse;
                        journeyModel.Flight.Add(modelResponse);
                    }
                    journeyRresponse.Add(journeyModel);
                }
            }
            return journeyRresponse;

        }
        private List<JourneyDTO> BuscarTrayectoBD(string origin, string destination)
        {
            return ListAll().Where(x => x.Origin == origin && x.Destination == destination).ToList();
        }

        public void GuargarTodo(List<JourneyResponse> responses)
        {
            bool Error = false;
            foreach (JourneyResponse itemJourney in responses)
            {
                List<int> listFlightId = new List<int>();
                foreach (FlightResponse itemFlight in itemJourney.Flight)
                {
                    FlightDTO FlightModel = _mapper.Map<FlightDTO>(itemFlight);
                    //Almacenar transport si no existe
                    TransportDTO transportModel = _mapper.Map<TransportDTO>(itemFlight.Transport);
                    FlightModel.IdTransport = ((ITransport)_transportBLL).Crear(transportModel);
                    if (FlightModel.IdTransport == -1)
                    {
                        Error = true;
                    }
                    if (!Error)
                    {
                        //Almacenar vuelos si no existe
                        int FlightId = ((IFlight)_flightBLL).Crear(FlightModel);
                        listFlightId.Add(FlightId);
                        if (FlightId == -1)
                        {
                            Error = true;
                        }
                    }

                }
                if (!Error)
                {
                    JourneyDTO journeyModel = _mapper.Map<JourneyDTO>(itemJourney);
                    journeyModel.IdJourney = Crear(journeyModel);
                    if (journeyModel.IdJourney == -1)
                    {
                        Error = true;
                    }
                    if (!Error)
                    {
                        foreach (int FlightId in listFlightId)
                        {
                            ((IJourneyFlight)_journeyFlightBLL).Crear(new JourneyFlightDTO()
                            {
                                IdJourney = journeyModel.IdJourney,
                                IdFlight = FlightId
                            });
                        }
                    }
                }
            }
      
        }
    }
}
