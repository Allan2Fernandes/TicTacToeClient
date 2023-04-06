using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeClient.Models;

namespace TicTacToeClient.ViewModels
{
    public partial class HighScoresViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string myWinsLabelString = "My Wins";

        [ObservableProperty]
        private string opponentWinsLabelString = "Opponent wins";       

        public HighScoresViewModel()
        {
        }

        // Method to handle the event
        public void HandleMessage(object sender, MessageEventArgs args)
        {
            Debug.WriteLine("In the subscriber's event");
            Debug.WriteLine(args.Message);
            OpponentWinsLabelString = args.Message;
            OnPropertyChanged(nameof(OpponentWinsLabelString));
        }
    }
}
