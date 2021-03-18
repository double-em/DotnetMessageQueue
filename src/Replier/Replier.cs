using System;
using System.Data.Common;
using System.Messaging;

namespace Replier
{
    public class Replier
    {
        private MessageQueue invalidQueue;

        public Replier(String requestQueueName, String invalidQueueName)
        {
            MessageQueue requestQueue = new MessageQueue(requestQueueName);
            invalidQueue = new MessageQueue(invalidQueueName);
            requestQueue.MessageReadPropertyFilter.SetAll();
            ((XmlMessageFormatter) requestQueue.Formatter).TargetTypeNames = new[] {"System.String,mscorlib"};
            requestQueue.ReceiveCompleted += OnReceiveCompleted;
            requestQueue.BeginReceive();
        }

        public void OnReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue requestQueue = (MessageQueue) source;
            Message requestMessage = requestQueue.EndReceive(asyncResult.AsyncResult);
            try
            {
                Console.WriteLine($"Received message: {requestMessage.Body}");
                
                // Console.WriteLine("Received        request");
                // Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                // Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                // Console.WriteLine("\tCorrel. ID: {0}", "<n/a>");
                // Console.WriteLine("\tReply to:   {0}", requestMessage.ResponseQueue.Path);
                // Console.WriteLine("\tContents:   {0}", requestMessage.Body);
                
                // string contents = requestMessage.Body.ToString();
                // MessageQueue replyQueue = requestMessage.ResponseQueue;
                // Message replyMessage = new Message();
                //
                // replyMessage.Body = contents;
                // replyMessage.CorrelationId = requestMessage.Id;
                // replyQueue.Send(replyMessage);
                //
                // Console.WriteLine("Sent        reply");
                // Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                // Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
                // Console.WriteLine("\tCorrel.        ID:        {0}", replyMessage.CorrelationId);
                // Console.WriteLine("\tReply to:   {0}", "<n/a>");
                // Console.WriteLine("\tContents:   {0}", replyMessage.Body);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid        message        detected");
                Console.WriteLine("\tType:       {0}", requestMessage.BodyType);
                Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                Console.WriteLine("\tCorrel. ID: {0}", "<n/a>");
                Console.WriteLine("\tReply to:   {0}", "<n/a>");
                requestMessage.CorrelationId = requestMessage.Id;
                invalidQueue.Send(requestMessage);
                Console.WriteLine("Sent to invalid message queue");
                Console.WriteLine("\tType:       {0}", requestMessage.BodyType);
                Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                Console.WriteLine("\tCorrel.        ID:        {0}", requestMessage.CorrelationId);
                Console.WriteLine("\tReply to:   {0}", requestMessage.ResponseQueue.Path);
            }

            requestQueue.BeginReceive();
        }
    }
}