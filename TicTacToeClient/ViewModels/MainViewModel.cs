using TicTacToeClient.Models;

namespace TicTacToeClient.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    ConnectionHandler ConnectionHandler;

    [ObservableProperty]
    private string[,] gameStates = new string[3, 3];



    public MainViewModel() 
    {
        ConnectionHandler = ConnectionHandler.getConnection();

        resetBoardMethod();
        
    }



    public string ConvertGameStatesToString()
    {
        string BoardAsString = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (GameStates[i,j] == "")
                {
                    BoardAsString += "_";
                }
                else
                {
                    BoardAsString += GameStates[i, j];
                }
            }
        }
        return BoardAsString;
    }

    [RelayCommand]
    private void UpdateServer(string parameter)
    {
        int xCoord = int.Parse(parameter[0].ToString());
        int yCoord = int.Parse(parameter[1].ToString());
        GameStates[xCoord, yCoord] = "X";
        ConnectionHandler.sendMessage(ConvertGameStatesToString());
        //return Task.CompletedTask;
    }

    [RelayCommand]
    private void ResetBoard()
    {
        resetBoardMethod();
    }

    private void resetBoardMethod()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameStates[i, j] = "";
            }
        }
    }


}

public class GameStatesIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string[,] Board = value as string[,];
        int xCoord = int.Parse(parameter.ToString()[0].ToString());
        int yCoord = int.Parse(parameter.ToString()[1].ToString());
        string BoardElement = Board[xCoord, yCoord];
        return BoardElement;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
