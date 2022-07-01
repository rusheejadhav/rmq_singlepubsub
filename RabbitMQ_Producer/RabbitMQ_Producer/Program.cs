using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Producer
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

            var messege = new { Sender = "Producer_01", Messege = "Hello from Producer_01 !!!" };    // annonymus object
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messege));    // converting JSON object into byte array by JsonConvert

            channel.BasicPublish("", "test01", null, body);
            // currently writing exchange as empty.
            // routing key = name of the queue.
        }
    }
}