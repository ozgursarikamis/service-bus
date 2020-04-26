using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus.Receiver
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

        private static async Task MainAsync()
        {
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();
            await _queueClient.CloseAsync();
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1, // handle one-by-one 
                AutoComplete = false
            };
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionArgs)
        {
            var exception = exceptionArgs.Exception;
            var context = exceptionArgs.ExceptionReceivedContext;
            var endpoint = context.Endpoint;
            var path = context.EntityPath;
            var action = context.Action;

            Console.WriteLine($"Exception: {exception.Message}");
            Console.WriteLine($"Endpoint: {endpoint}");
            Console.WriteLine($"Path: {path}");
            Console.WriteLine($"Action: {action}");
            return Task.CompletedTask;

        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received Message: Sequence Number: {message.SystemProperties.SequenceNumber}");
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}