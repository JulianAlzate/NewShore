using API.Model;
using AutoMapper;
using BLL.Common;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Response;
using DataBase.DbManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static BLL.Common.Enums;

namespace BLL.RN
{
    public class FlightBLL : IFlight
    {
        private readonly IMapper _mapper;
        public Repositorio<Flight> _repo;

        public FlightBLL(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new Repositorio<Flight>(new NewShoreContext());

        }

        public FlightDTO BuscarPorId(params object[] keyValues)
        {
            Flight modelBD = _repo.BuscarPorId(keyValues);
            FlightDTO modelDTO = _mapper.Map<FlightDTO>(modelBD);
            return modelDTO;

        }

        public int Crear(FlightDTO model)
        {
            FlightDTO existDB = ListAll().FirstOrDefault(x => x.Origin == model.Origin && x.Destination == model.Destination);
            if (existDB == null)
            {
                Flight modelDB = _mapper.Map<Flight>(model);
                _repo.Crear(modelDB);
                _repo.Confirmar();
                return modelDB.IdFlight;
            }
            return existDB.IdFlight;
        }

        public bool Existe(FlightDTO model)
        {
            throw new NotImplementedException();
        }

        public IList<FlightDTO> ListAll()
        {
            List<Flight> modelBD = _repo.Listar.ToList();
            List<FlightDTO> modelDTO = _mapper.Map<List<FlightDTO>>(modelBD);
            return modelDTO;
        }

        public List<FlightResponse> ConsultarTodosLosVuelos(TipoVuelo codApiVuelo)
        {
            var url = "https://recruiting-api.newshore.es/api/flights/" + codApiVuelo.GetHashCode();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            var responseBody = objReader.ReadToEnd();

                            //JObject json = JObject.Parse(responseBody);
                            List<FlightResponse> list = JsonConvert.DeserializeObject<List<FlightResponse>>(responseBody);
                            // Do something with responseBody
                            return list;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                //PDT LOG
                return new List<FlightResponse>();
            }


        }

    }
}
