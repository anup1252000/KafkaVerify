using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaVerify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ProducerConfig config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ProducerConfig config)
        {
            _logger = logger;
            this.config = config;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                for (int i = 0; i < 100; ++i)
                {

                    p.Produce("order", new Message<Null, string> { Value = "Test" + i.ToString() });
                    //Thread.Sleep(3000);

                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
            return Ok();
        }
    }
}
