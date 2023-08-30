using OrderService.DTOs;
using OrderService.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            //var factory = new ConnectionFactory()
            //{
            //    HostName = _configuration["RabbitMQHost"],
            //    Port = int.Parse(_configuration["RabbitMQPort"]),
            //    UserName = "guest",
            //    Password = "guest"
            //    //Uri = new Uri("amqp://guest:guest@127.0.0.1:5672")
            //};
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                //Subscribing to the Connection Shutdown event
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("-- Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message bus: {ex.Message}");
            }

        }
        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ ConnectionShutdown");
        }

        public void PublishNewOrder(OrderPublishedDto order)
        {
            var message = JsonSerializer.Serialize(order);

            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabbitMQ Connection Shutdown");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                                    routingKey: "",
                                    basicProperties: null,
                                    body: body);
            Console.WriteLine($"----> Message {message} was sent");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
