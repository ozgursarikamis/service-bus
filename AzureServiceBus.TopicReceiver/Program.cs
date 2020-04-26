using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus.TopicReceiver
{
    internal static class Program
    {
        
        private const string ServiceBusConnectionString = "Endpoint=sb://ozgur-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/J/XVuxJIyVlBr8qoq0TR703gHLIatE268WIfklS/Nk=";
        private const string SubscriptionName = "subs1";
        private const string TopicName = "topic1";
        private static ISubscriptionClient _subscriptionClient;
        
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            _subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            RegisterMessageHandler();
            Console.ReadKey();
            await _subscriptionClient.CloseAsync();
        }

        private static void RegisterMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionHandler);
            _subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine(arg.Exception.Message);
            Console.WriteLine(arg.ExceptionReceivedContext.Action);
            Console.WriteLine(arg.ExceptionReceivedContext.Endpoint);
            Console.WriteLine(arg.ExceptionReceivedContext.EntityPath);
            return Task.CompletedTask;
        }

        private static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"SequenceNumber : {message.SystemProperties.SequenceNumber}");
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}