using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace privatemessagesender
{
    class Program
    {

        const string ServiceBusConnectionString = "";
        const string QueueName = "salesmessages";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            string senderName = "Default Sender";
            if (args?.Length > 0)
                senderName = $"Sender {args[0]}";
            Console.WriteLine("Sending a message to the Sales Messages queue...");

            SendSalesMessageAsync(senderName, 10).GetAwaiter().GetResult();
            //SendSalesMessagesAsync(senderName).GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync(string senderName, int repeatTimes = 1)
        {
            // Create a Queue Client here
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            // Send messages.
            try
            {
                // Create and send a message here
                string messageBody = $"[{senderName}] $10,000 order for bicycle parts from retailer Adventure Works.";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                for (int i = 0; i < repeatTimes; i++)
                {
                    await queueClient.SendAsync(message);
                    Console.WriteLine($"Sending message: {messageBody} for {i} times");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            // Close the connection to the queue here
            await queueClient.CloseAsync();
        }

        static async Task SendSalesMessagesAsync(string senderName)
        {
            // Create a Queue Client here
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            // Send messages.
            try
            {
                // Create and send a message here
                string messageBody = $"[{senderName}] $10,000 order for bicycle parts from retailer Adventure Works.";
                List<Message> messages = new List<Message>();
                for (int i = 0; i < 10; i++)
                {
                    messages.Add(new Message(Encoding.UTF8.GetBytes(messageBody)));
                }
                await queueClient.SendAsync(messages);
                Console.WriteLine($"Sending 10 messages for one time");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            // Close the connection to the queue here
            await queueClient.CloseAsync();
        }
    }
}
