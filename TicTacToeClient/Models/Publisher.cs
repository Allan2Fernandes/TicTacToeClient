using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeClient.Models
{
    public class Publisher
    {
        // Declare the event
        public event EventHandler<MessageEventArgs> MessageEvent;

        // Method to raise the event
        public void SendMessage(string message)
        {
            MessageEventArgs args = new MessageEventArgs { Message = message };
            OnMessageEvent(args);
        }

        // Protected method to raise the event
        protected virtual void OnMessageEvent(MessageEventArgs args)
        {
            MessageEvent?.Invoke(this, args);
        }
    }
}
