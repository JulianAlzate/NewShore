using BLL.Dto;
using BLL.Response;
using System;
using System.Collections.Generic;
using System.Text;
using static BLL.Common.Enums;

namespace BLL.Interfaces
{
    public interface IFlight : IBusinessLogic<FlightDTO>
    {
        public List<FlightResponse> ConsultarTodosLosVuelos(TipoVuelo codApiVuelo);

    }
}
