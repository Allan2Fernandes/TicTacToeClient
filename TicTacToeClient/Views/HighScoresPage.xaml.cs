namespace TicTacToeClient.Views;

public partial class HighScoresPage : ContentPage
{
	public HighScoresPage(HighScoresViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}