using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus.TopicSender
{
    internal static class Program
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://ozgur-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/J/XVuxJIyVlBr8qoq0TR703gHLIatE268WIfklS/Nk=";
        private const string TopicName = "topic1";
        private static ITopicClient _topicClient;
        
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            _topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            await SendMessageAsync(numberOfMessages);
            Console.ReadKey();
            await _topicClient.CloseAsync();
        }

        private static async Task SendMessageAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (int i = 0; i < numberOfMessagesToSend; i++)
                {
                    string messageBody = $"Message: {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    Console.WriteLine($"Sending message: {messageBody}");
                    await _topicClient.SendAsync(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}