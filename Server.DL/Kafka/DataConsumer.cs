using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.DL.DataFlow;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.DL.Kafka
{
    public class DataConsumer : BackgroundService
    {
        private readonly ILogger<DataConsumer> _logger;
        private readonly ConsumerConfig _kafkaConfig;
        private readonly IComputerDataFlow _computerDataFlow;

        public DataConsumer(ILogger<DataConsumer> logger, IComputerDataFlow computerDataFlow)
        {
            _logger = logger;
            _computerDataFlow = computerDataFlow;

            _kafkaConfig = new ConsumerConfig
            {
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 5000,
                FetchWaitMaxMs = 50,
                BootstrapServers = "localhost:9092",
                GroupId = $"ComputerServer",
                AutoOffsetReset = AutoOffsetReset.Latest,
                ClientId = "2"
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<int, byte[]>(_kafkaConfig).Build();

                try
                {
                    consumer.Subscribe("data");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(stoppingToken);

                            try
                            {
                                _computerDataFlow.ProcessMessage(consumeResult.Message.Value);

                                _logger.LogInformation(consumeResult.Message.Key.ToString());
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Kafka consume message error: {0}", ex.Message);
                            }

                            if (consumeResult.IsPartitionEOF)
                                break;

                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError(e, $"Consumer for topic '{e.ConsumerRecord.Topic}'. ConsumeException, Key: {Encoding.UTF8.GetString(e.ConsumerRecord.Message.Key)}, Error: {JsonConvert.SerializeObject(e.Error)}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Kafka consume message error: {0}", ex.Message);
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogError(e, $"Consumer for topics data: {e.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Consumer for topics '{string.Join(';', "data")}'. Exception.");
                }
            }, stoppingToken);
        }
    }
}

