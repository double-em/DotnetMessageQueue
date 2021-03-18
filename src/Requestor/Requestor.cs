using System;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Requestor
{
    public class Requestor
    {
        private MessageQueue requestQueue;
        private MessageQueue replyQueue;

        public Requestor(String requestQueueName, String replyQueueName)
        {
            requestQueue = new MessageQueue(requestQueueName);
            requestQueue.MessageReadPropertyFilter.SetAll();
            
            replyQueue = new MessageQueue(replyQueueName);
            replyQueue.MessageReadPropertyFilter.SetAll();
            
            ((XmlMessageFormatter) replyQueue.Formatter).TargetTypeNames = new[] {"System.String,mscorlib"};
            replyQueue.ReceiveCompleted += ReplyQueueOnReceiveCompleted;
        }

        private static void ReplyQueueOnReceiveCompleted(object sender, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                var messageQueue = (MessageQueue) sender;
                
                var message = messageQueue.EndReceive(asyncResult.AsyncResult);
                
                Console.WriteLine("Received        reply");
                Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", message.Id);
                Console.WriteLine("\tCorrel. ID: {0}", message.CorrelationId);
                Console.WriteLine("\tReply to:   {0}", "<n/a>");
                Console.WriteLine("\tContents:   {0}", message.Body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Send(string messageId)
        {
            Message requestMessage = new Message();
            requestMessage.AcknowledgeType = AcknowledgeTypes.PositiveReceive | AcknowledgeTypes.PositiveArrival;
            requestMessage.Body = messageId;
            requestMessage.ResponseQueue = replyQueue;
            
            requestQueue.Send(requestMessage);
            
            Console.WriteLine("Sent        request");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", requestMessage.CorrelationId);
            Console.WriteLine("\tReply to:   {0}", requestMessage.ResponseQueue.Path);
            Console.WriteLine("\tContents:   {0}", requestMessage.Body);
        }

        public WaitHandle ReceiveAsync()
        {
            return requestQueue.BeginReceive().AsyncWaitHandle;
        }

        public void ReceiveSync()
        {
            Message replyMessage = replyQueue.Receive();
            
            Console.WriteLine("Received        reply");
            Console.WriteLine("\tTime:       {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", replyMessage.CorrelationId);
            Console.WriteLine("\tReply to:   {0}", "<n/a>");
            Console.WriteLine("\tContents:   {0}", replyMessage.Body.ToString());
        }
    }
}