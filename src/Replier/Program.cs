using System;
using System.Threading;

namespace Replier
{
    internal class Program
    {
        public static void Main()
        {
            var replier = new Replier(_queueReplyPath, _queueInvalidPath);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        
        private static readonly string _queueReplyPath = @".\private$\myQueue";
        private static readonly string _queueInvalidPath = @".\private$\inValidQueue";
    }
}