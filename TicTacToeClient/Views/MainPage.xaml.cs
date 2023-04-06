using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace TicTacToeClient.Views;

public partial class MainPage : ContentPage
{
    //HubConnection connection;

    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;       
    }    

}
