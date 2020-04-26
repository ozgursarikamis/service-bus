using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus
{
    internal static class Program
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://ozgur-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/J/XVuxJIyVlBr8qoq0TR703gHLIatE268WIfklS/Nk=";
        private const string QueueName = "queue1";
        private static IQueueClient _queueClient;
        
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        // Write messages to queue:
        private static async Task MainAsync()
        {
            const int numberOfMessagesToSend = 10;
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            await SendMessageAsync(numberOfMessagesToSend);
            await _queueClient.CloseAsync();
        }

        static async Task SendMessageAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (int i = 0; i < numberOfMessagesToSend; i++)
                {
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    Console.WriteLine($"Sending Message: { messageBody }");
                    await _queueClient.SendAsync(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}