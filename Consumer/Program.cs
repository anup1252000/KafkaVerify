using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<ConsumerConfig>(option =>
                    {
                        ConsumerConfig config = new()
                        {
                            BootstrapServers = "pkc-lzvrd.us-west4.gcp.confluent.cloud:9092",
                            SecurityProtocol = SecurityProtocol.SaslSsl,
                            SaslUsername = "AH4TX4OKCUMHI3GV",
                            SaslPassword = "HnWSuS8/rGaNJwNpliy0lmwUYHWL8+eHYn26GqcOvh9qPTfpzUDiNVKQlhH+ddSb",
                            SaslMechanism = SaslMechanism.Plain,
                            AutoOffsetReset = AutoOffsetReset.Earliest,
                            GroupId = Guid.NewGuid().ToString(),
                            EnableAutoCommit=false,
                            MaxPollIntervalMs=300000,
                            EnableAutoOffsetStore=false
                        };
                        return config;
                    });
                });
    }
}
