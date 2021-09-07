using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SerilogDemo.Api2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {
        private static readonly string[] Status = new[]
        {
            "Blue", "Yellow", "Red"
        };

        private readonly ILogger<TrafficController> _logger;

        public TrafficController(ILogger<TrafficController> logger)
        {
            _logger = logger;
        }

     
         [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var complexObject = new TraficReport
                {
                    Date = DateTime.Now,
                    PoliceRequired = true,
                    Status = "Red"
                };

                _logger.LogInformation("This is an example of a complex object: {@ComplexObject}", complexObject);

                throw new Exception("Some Error Incomming!!!");

                return Ok(CreateResponse());
            }
            catch(Exception ex)
            {
               
                _logger.LogError("Some error ocurred: {@Ex}",ex);

                return BadRequest(JsonConvert.SerializeObject(ex));
            }
        }

        private IEnumerable<TraficReport> CreateResponse()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new TraficReport
            {
                Date = DateTime.Now.AddDays(index),
                PoliceRequired = true,
                Status = Status[rng.Next(Status.Length)]
            })
            .ToArray();
        }
    }
}
