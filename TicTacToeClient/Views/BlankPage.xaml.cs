﻿namespace TicTacToeClient.Views;

public partial class BlankPage : ContentPage
{
	public BlankPage(BlankViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
