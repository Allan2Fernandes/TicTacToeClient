using TicTacToeClient.Models;

namespace TicTacToeClient.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    ConnectionHandler ConnectionHandler;

    [ObservableProperty]
    private string[,] gameStates = new string[3, 3];

    bool IsPlayer1Turn;



    public MainViewModel() 
    {
        ConnectionHandler = ConnectionHandler.getConnection();
        ResetGameStatesMethod();        
    }



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

    [RelayCommand]
    private async Task UpdateServer(string parameter)
    {
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
        IsPlayer1Turn = !IsPlayer1Turn;
        await ConnectionHandler.sendMessage(ConvertGameStatesToString());
    }

    [RelayCommand]
    private async Task ResetGameStates()
    {
        ResetGameStatesMethod();
        //Update the server with the reset board
        await ConnectionHandler.sendMessage(ConvertGameStatesToString());
    }

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
    }


}

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
