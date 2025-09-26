using Application.Common.Interfaces;
using Application.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Implementation
{
    public class RabbitMQProducer : IMessageQueueService
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMQSettings _rabbitMQSettings;

        public RabbitMQProducer(IOptions<RabbitMQSettings> options)
        {
            _rabbitMQSettings = options.Value;

            _factory = new ConnectionFactory() { 
                HostName = _rabbitMQSettings.Host,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password,
                Port = _rabbitMQSettings.Port
            };
        }

        public async void SendMessage<T>(T message, string queueName)
        {   
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ Error: {ex.Message}");
            }
        }
    }
}
