using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumerService.Consumer
{
    public abstract class BaseRabbitMqConsumer : BackgroundService
    {
        private readonly ILogger<BaseRabbitMqConsumer> _logger;
        private readonly string _queueName;
        private IConnection _connection;
        private IChannel _channel;

        protected BaseRabbitMqConsumer(string queueName, ILogger<BaseRabbitMqConsumer> logger)
        {
            _queueName = queueName;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = "localhost",
                        UserName = "guest",
                        Password = "guest",
                        Port = 5672
                    };

                    _connection = await factory.CreateConnectionAsync();
                    _channel = await _connection.CreateChannelAsync();
                    await _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(_channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogInformation($"Received message from queue '{_queueName}': {message}");

                        try
                        {
                            await ProcessMessageAsync(message);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error processing message from queue '{_queueName}': {ex.Message}");
                        }
                    };

                    await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RabbitMQ connection failed: {ex.Message}");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        protected abstract Task ProcessMessageAsync(string message);

        public override async void Dispose()
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
            base.Dispose();
        }
    }
}
