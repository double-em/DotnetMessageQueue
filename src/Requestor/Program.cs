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
                
                for (int i = count - 1; i >= 0; i--)
                {
                    var messageId = (i + 1).ToString();
                    requestor.Send(messageId);
                    Console.WriteLine($"Send message: {messageId}");
                    // waitHandleArray[i] = requestor.ReceiveAsync();
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
