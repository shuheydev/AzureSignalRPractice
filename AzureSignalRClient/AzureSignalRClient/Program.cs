using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace AzureSignalRClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            HubConnection connection;
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chat")
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string>("broadcastMessage", (string name, string message) =>
            {
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                Console.WriteLine($"{name}: {message}");
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("接続できました");
            }
            catch (Exception ex)
            {
                Console.WriteLine("接続できませんでした");
            }

            string input;
            while (true)
            {
                if ((input = Console.ReadLine()) == "exit")
                    break;

                await connection.InvokeAsync("BroadcastMessage", "console", input);
            }
        }
    }
}