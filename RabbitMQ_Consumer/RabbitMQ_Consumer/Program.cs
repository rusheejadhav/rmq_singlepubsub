using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ_Consumer
{
    static class Program
    {
        static void Main(String[] args)
        {
            // create connection factory
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
                // rmq listen on port 5672
                // "amqp://{{username}}:{{password}}@{{host}}:{{port}}"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();   // creates and return a fresh channel, session and model

            channel.QueueDeclare("test01",
                durable: true,  // messege will stay until a consumer reads it
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);  // takes IModel as an argument in a constructor
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();    // return byte array of messeges
                var messege = Encoding.UTF8.GetString(body);
                Console.WriteLine(messege); // printing json string as it is
            };

            channel.BasicConsume("test01", true, consumer);
            Console.ReadLine();
        }
    }
}