using Microsoft.AspNetCore.SignalR.Client;
using TicTacToeClient.Models;

namespace TicTacToeClient.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    private static HubConnection connection;

    [ObservableProperty]
    private string[,] gameStates = new string[3, 3];

    [ObservableProperty]
    private string myWinsLabel = PlayerDetails.Name + " Wins = 0";

    [ObservableProperty]
    private string opponentWinsLabel = "Opponent Wins = 0";

    bool IsPlayer1Turn;
    bool AmIWinner = false;
    bool IsOpponentWinner = false;
    int numOpponentWins = 0;
    int numMyWins = 0;

    Publisher publisher = new Publisher();
    //The subscriber
    HighScoresViewModel highScoresViewModel = new HighScoresViewModel();
    


    public MainViewModel() 
    {
        //Set up the connection here
        connection = new HubConnectionBuilder()
            .WithUrl("http://192.168.1.24:5157/messageHub")
            .Build();

        ResetGameStatesMethod();
        ReceiveBroadCastedMethod();
        publisher.MessageEvent += highScoresViewModel.HandleMessage;
    }

    //Test if the game should end
    public bool WinnerShouldBeDeclared()
    {
        string SymbolToCheck = IsPlayer1Turn?"X":"O";
       
        //Check all rows
        if (
            (GameStates[0, 0] == SymbolToCheck && GameStates[1, 0] == SymbolToCheck && GameStates[2, 0] == SymbolToCheck)
            ||
            (GameStates[0, 1] == SymbolToCheck && GameStates[1, 1] == SymbolToCheck && GameStates[2, 1] == SymbolToCheck)
            ||
            (GameStates[0, 2] == SymbolToCheck && GameStates[1, 2] == SymbolToCheck && GameStates[2, 2] == SymbolToCheck)
            )
        {
            Debug.WriteLine("Winner from Row Checks");
            AmIWinner = true;
            numMyWins++;
            PlayerDetails.MyWins = numMyWins;
            MyWinsLabel = PlayerDetails.Name + "Wins = " + PlayerDetails.MyWins;
            OnPropertyChanged(nameof(MyWinsLabel));
            return true;
        }
        //Check all columns
        if (
             (GameStates[0, 0] == SymbolToCheck && GameStates[0, 1] == SymbolToCheck && GameStates[0, 2] == SymbolToCheck)
            ||
            (GameStates[1, 0] == SymbolToCheck && GameStates[1, 1] == SymbolToCheck && GameStates[1, 2] == SymbolToCheck)
            ||
            (GameStates[2, 0] == SymbolToCheck && GameStates[2, 1] == SymbolToCheck && GameStates[2, 2] == SymbolToCheck)
            )
        {
            Debug.WriteLine("Winner from Column Checks");
            AmIWinner = true;
            numMyWins++;
            PlayerDetails.MyWins = numMyWins;
            MyWinsLabel = PlayerDetails.Name + "Wins = " + PlayerDetails.MyWins;
            OnPropertyChanged(nameof(MyWinsLabel));
            return true;
        }
        //Check both diagonals
        if (
            (GameStates[0, 0] == SymbolToCheck && GameStates[1, 1] == SymbolToCheck && GameStates[2,2] == SymbolToCheck)
            ||
            (GameStates[2, 0] == SymbolToCheck && GameStates[1, 1] == SymbolToCheck && GameStates[0, 2] == SymbolToCheck)
            )
        {
            Debug.WriteLine("Winner From Diagonals");
            AmIWinner = true;
            numMyWins++;
            PlayerDetails.MyWins = numMyWins;
            MyWinsLabel = PlayerDetails.Name + "Wins = " + PlayerDetails.MyWins;
            OnPropertyChanged(nameof(MyWinsLabel));
            return true;
        }


        return false;
    }


    //SignalR methods
    public void ReceiveBroadCastedMethod()
    {

        //Set it up to receive messages
        try
        {
            connection.On<string, string, bool>("ReceiveMessage", (GameStateString, userTurn, ThereIsWinner) =>
            {
                GameStates = ConvertStringBackToGameStates(GameStateString);
                if(userTurn == "P1")
                {
                    IsPlayer1Turn = true;
                }
                else
                {
                    IsPlayer1Turn = false;
                }
                IsOpponentWinner = ThereIsWinner;
                if (IsOpponentWinner && !AmIWinner) //Make sure you aren't updating both scores together
                {
                    numOpponentWins++;
                    PlayerDetails.OpponentWins = numOpponentWins;
                    OpponentWinsLabel = "Opponent Wins = " + PlayerDetails.OpponentWins;
                    publisher.SendMessage("Opponent Wins = " + PlayerDetails.OpponentWins);
                    OnPropertyChanged(nameof(OpponentWinsLabel));
                } else if (!IsOpponentWinner)
                {
                    //Reset the game
                    AmIWinner = false;
                    IsOpponentWinner = false;
                }
            });

            connection.StartAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        OnPropertyChanged(nameof(GameStates));
    }

    public async Task sendMessage(string GameStateString, string userTurn, bool ThereIsWinner)
    {
        await connection.InvokeAsync("SendMessage", GameStateString, userTurn, ThereIsWinner);
    }


    //Data conversion methods to send and receive data
    public string ConvertGameStatesToString()
    {
        string GameStatesAsString = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (GameStates[i,j] == "")
                {
                    GameStatesAsString += "_";
                }
                else
                {
                    GameStatesAsString += GameStates[i, j];
                }
            }
        }
        return GameStatesAsString;
    }

    public string[,] ConvertStringBackToGameStates(string GameStatesAsString)
    {
        int index = 0;
        string[,] ConvertedGameStates = new string[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (GameStatesAsString[index].ToString() == "_")
                {
                    ConvertedGameStates[i, j] = "";
                    index++;
                }
                else
                {
                    ConvertedGameStates[i, j] = GameStatesAsString[index++].ToString();
                }
                
            }
        }
        return ConvertedGameStates;
    }


    //Bound commands
    [RelayCommand]
    private async Task UpdateServer(string parameter)
    {
        if (IsOpponentWinner || AmIWinner)
        {
            Debug.WriteLine("Winner already declared");
            return;
        }
        int xCoord = int.Parse(parameter[0].ToString());
        int yCoord = int.Parse(parameter[1].ToString());
        if (GameStates[xCoord, yCoord] != "")
        {
            return;
        }
        if (IsPlayer1Turn)
        {
            GameStates[xCoord, yCoord] = "X";
        }
        else
        {
            GameStates[xCoord, yCoord] = "O";
        }
        WinnerShouldBeDeclared();
        IsPlayer1Turn = !IsPlayer1Turn;
        OnPropertyChanged(nameof(GameStates));
        await sendMessage(ConvertGameStatesToString(), IsPlayer1Turn?"P1":"P2", AmIWinner);
        
    }

    [RelayCommand]
    private async Task ResetGameStates()
    {
        ResetGameStatesMethod();
        //Update the server with the reset board
        await sendMessage(ConvertGameStatesToString(), "P1", false);
    }

    //Initialize method
    private void ResetGameStatesMethod()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameStates[i, j] = "";
            }
        }
        IsPlayer1Turn = true;
        AmIWinner = false;
        IsOpponentWinner= false;
        OnPropertyChanged(nameof(GameStates));
    }


}

//Binding converter
public class GameStatesIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        string[,] GameStates = value as string[,];
        int xCoord = int.Parse(parameter.ToString()[0].ToString());
        int yCoord = int.Parse(parameter.ToString()[1].ToString());
        string SingleElement = GameStates[xCoord, yCoord];
        return SingleElement;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
