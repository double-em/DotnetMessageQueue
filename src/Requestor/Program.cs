using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;

namespace Requestor
{
    public class MyQueue
    {
        public static void Main()
        {
            var requestor = new Requestor(_queueRequestPath, _queueReplyPath);

            while (true)
            {
                var count = 3;
                var waitHandleArray = new WaitHandle[count];

                var messages = new List<Message>();
                
                for (int i = 1; i <= count; i++)
                {
                    var messageId = i.ToString();
                    messages.Add(requestor.MessageConstructor(messageId));
                    Console.WriteLine($"Constructed message: {messageId}");
                    // waitHandleArray[i] = requestor.ReceiveAsync();
                }

                messages.Reverse();

                foreach (var message in messages)
                {
                    requestor.Send(message);
                }

                // Console.WriteLine("Waiting to receive...");
                // WaitHandle.WaitAll(waitHandleArray);

                // Console.WriteLine("Done receiving...");
                Console.ReadLine();
            }
        }

        private static readonly string _queueRequestPath = @".\private$\myQueue";
        private static readonly string _queueReplyPath = @".\private$\myReplyQueue";
        private readonly MessageQueue _queue = new MessageQueue(_queueRequestPath);

        public void SendPrivate()
        {
            var message = "Private queue by path name.";
            _queue.Send(message);
            Console.WriteLine($"[X][Name:] Send message: '{message}'");
        } 
    }
}
