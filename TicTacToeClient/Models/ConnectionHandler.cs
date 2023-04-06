using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace TicTacToeClient.Models
{
    public class ConnectionHandler
    {
        private static ConnectionHandler instance = null;
        private static HubConnection connection;
        private ConnectionHandler() 
        {
            //Set up the connection here
            connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.1.24:5157/messageHub")
                .Build();

            //Set it up to receive messages
            try
            {
                connection.On<string>("ReceiveMessage", (message) =>
                {
                    Debug.WriteLine(message);
                });

                connection.StartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        } 

        public static ConnectionHandler getConnection()
        {
            if(instance == null)
            {
                //Create instance
                instance = new ConnectionHandler();
            }

            return instance;
            
        }

        public async void sendMessage(string message)
        {
            await connection.InvokeAsync("SendMessage", message);
        }
    }
}
