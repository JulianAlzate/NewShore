using API.Model;
using AutoMapper;
using BLL.Dto;
using BLL.Interfaces;
using DataBase.DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.RN
{
    public class JourneyFlightBLL : IJourneyFlight
    {
        private readonly IMapper _mapper;
        public Repositorio<JourneyFlight> _repo;

        public JourneyFlightBLL(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new Repositorio<JourneyFlight>(new NewShoreContext());

        }
        public JourneyFlightDTO BuscarPorId(params object[] keyValues)
        {
            //JourneyFlight modelBD = _repo.Listar.Where(x => x.IdJourney == Convert.ToInt64(keyValues[0])).FirstOrDefault();
            //JourneyFlightDTO modelDTO = _mapper.Map<JourneyFlightDTO>(modelBD);
            //return modelDTO;
            throw new NotImplementedException();
        }

        public int Crear(JourneyFlightDTO model)
        {
            JourneyFlightDTO existDB = ListAll().FirstOrDefault(x => x.IdFlight == model.IdFlight
            && x.IdJourney == model.IdJourney);
            if (existDB == null)
            {
                JourneyFlight modelDB = _mapper.Map<JourneyFlight>(model);
                _repo.Crear(modelDB);
                _repo.Confirmar();
                return modelDB.IdJourneyFlight;
            }
            return existDB.IdJourneyFlight;
        }

        public IList<JourneyFlightDTO> ListAll()
        {
            List<JourneyFlight> modelBD = _repo.Listar.ToList();
            List<JourneyFlightDTO> modelDTO = _mapper.Map<List<JourneyFlightDTO>>(modelBD);
            return modelDTO;
        }
    }
}
