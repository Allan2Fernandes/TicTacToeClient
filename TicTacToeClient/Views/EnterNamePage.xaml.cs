using TicTacToeClient.Models;

namespace TicTacToeClient.Views;

public partial class EnterNamePage : ContentPage
{
	public EnterNamePage(EnterNameViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        PlayerDetails.Name = NameEntryField.Text;
        NameEntryField.Text= string.Empty;
    }
}