using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ConsumerConfig config;
        private readonly ILogger<Worker> logger;

        public Worker(ConsumerConfig config, ILogger<Worker> logger)
        {
            this.config = config;
            this.logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var consumerClient = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerClient.Subscribe("order");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = consumerClient.Consume(stoppingToken);
                    logger.LogInformation($"consumed message value:{cr.Message.Value}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
