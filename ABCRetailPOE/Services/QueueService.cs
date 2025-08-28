using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ABCRetailPOE.Services
{
    public class QueueService
    {
        private readonly QueueServiceClient _queueServiceClient;

        public QueueService(QueueServiceClient queueServiceClient)
        {
            _queueServiceClient = queueServiceClient;
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

        public async Task<string?> ReceiveMessageAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            if (!await queueClient.ExistsAsync()) return null;

            QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(1);
            if (messages.Length > 0)
            {
                var msg = messages[0];
                await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                return msg.MessageText;
            }
            return null;
        }
    }
}
