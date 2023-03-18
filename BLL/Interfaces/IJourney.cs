using BLL.Common;
using BLL.Dto;
using BLL.Response;
using System;
using System.Collections.Generic;
using System.Text;
using static BLL.Common.Enums;

namespace BLL.Interfaces
{
    public interface IJourney
    {
        public List<JourneyResponse> ObtenerVuelos(RequestFilter requestFilter);
    }
}
