using API.Model;
using BLL.Common;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Response;
using BLL.RN;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {
        private readonly IBusinessLogic<JourneyDTO> _journeyBLL;
        public IConfigurationRoot Configuration { get; set; }
        public JourneyController(IBusinessLogic<JourneyDTO> journeyBLL)
        {
            _journeyBLL = journeyBLL;
        }

        // GET: api/<JourneyController>
        [HttpGet("{origin}/{destination}/{conEscala?}/{conRegreso?}")]
        public IActionResult Get(string origin, string destination, bool conEscala = false,bool conRegreso = false)
        {

            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
            {
                return NotFound("No hay ingresado la informacion de origen o destino.");

            }
            RequestFilter requestFilter = new RequestFilter()
            {
                origin = origin,
                destination = destination,
                conMultiple = conEscala,
                conRetorno = conRegreso
            };
            
            List<JourneyResponse> responseUser = ((IJourney)_journeyBLL).ObtenerVuelos(requestFilter);
            if(responseUser== null || responseUser.Count == 0)
            {
                //MENSAJE AL USUARIO
                return NotFound("La ruta no puede ser calculada.");
            }

            return Ok(responseUser);


        }


        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<JourneyController>/5
        //[HttpGet("{Origin}/{Destination}")]
        //public string Get(string Origin, string Destination)
        //{
        //    return "value";
        //}

        //// POST api/<JourneyController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<JourneyController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<JourneyController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
