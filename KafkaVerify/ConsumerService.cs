using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaVerify
{
    public class ConsumerService : BackgroundService
    {
        private readonly ConsumerConfig config;
        private readonly ILogger<ConsumerService> logger;

        public ConsumerService(ConsumerConfig config,ILogger<ConsumerService> logger)
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
                    var cr=consumerClient.Consume(stoppingToken);
                    logger.LogInformation($"consumed message value:{cr.Message.Value}");
                    if (cr?.Message?.Value != null)
                    {
                        consumerClient.Commit(cr);
                        consumerClient.StoreOffset(cr);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}