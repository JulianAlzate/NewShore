using API.Model;
using BLL.Common;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Response;
using BLL.RN;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {
        private readonly IBusinessLogic<JourneyDTO> _journeyBLL;
        protected static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IConfigurationRoot Configuration { get; set; }
        public JourneyController(IBusinessLogic<JourneyDTO> journeyBLL)
        {
            _journeyBLL = journeyBLL;
        }

        // GET: api/<JourneyController>
        [HttpGet("{origin}/{destination}/{conEscala?}/{conRegreso?}")]
        public IActionResult Get(string origin, string destination, bool conEscala = false, bool conRegreso = false)
        {

            string mensaje = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
                {
                    return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), "No hay ingresado la informacion de origen o destino.");
                }
                else
                {

                    RequestFilter requestFilter = new RequestFilter()
                    {
                        origin = origin,
                        destination = destination,
                        withMultiple = conEscala,
                        withReturn = conRegreso
                    };

                    List<JourneyResponse> responseUser = ((IJourney)_journeyBLL).ObtenerVuelos(requestFilter);
                    if (responseUser == null || responseUser.Count == 0)
                    {
                        Logger.Error("No se pudo calcular la ruta");
                        return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), "La ruta no puede ser calculada.");

                    }
                    return Ok(responseUser);

                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);

            }

        }


    }
}
