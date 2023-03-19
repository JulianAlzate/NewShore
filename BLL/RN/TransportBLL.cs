
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
    public class TransportBLL : ITransport
    {
        private readonly IMapper _mapper;
        public Repositorio<Transport> _repo;
        public TransportBLL(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new Repositorio<Transport>(new NewShoreContext());

        }

        public TransportDTO BuscarPorId(params object[] keyValues)
        {
            Transport modelBD = _repo.BuscarPorId(keyValues);
            TransportDTO modelDTO = _mapper.Map<TransportDTO>(modelBD);
            return modelDTO;
        }

        public int Crear(TransportDTO model)
        {
            TransportDTO existDB = ListAll().FirstOrDefault(x => x.FlightCarrier == model.FlightCarrier && x.FlightNumber == model.FlightNumber);
            if (existDB == null)
            {
                Transport modelDB = _mapper.Map<Transport>(model);
                _repo.Crear(modelDB);
                _repo.Confirmar();
                return modelDB.IdTransport;
            }
            return existDB.IdTransport;
        }

        public bool Existe(TransportDTO model)
        {
            throw new NotImplementedException();
        }

        public IList<TransportDTO> ListAll()
        {

            List<Transport> modelBD = _repo.Listar.ToList();
            List<TransportDTO> modelDTO = _mapper.Map<List<TransportDTO>>(modelBD);
            return modelDTO;
        }

    }

}

