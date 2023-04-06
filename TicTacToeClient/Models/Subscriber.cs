using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeClient.Models
{
    // Define the subscriber class
    public class Subscriber
    {
        // Method to handle the event
        public void HandleMessage(object sender, MessageEventArgs args)
        {
            Debug.WriteLine(args.Message);
        }
    }
}
