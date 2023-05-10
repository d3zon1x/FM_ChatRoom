using ExpansionForCAndS;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ServerHandler : SingletonClass<ServerHandler>
    {
        public ObservableCollection<User> users;
        private string serverAddress = "127.0.0.1";
        private int port = 4040;
        private Socket server;
        public bool IsConnected => server.Connected;
        public event EventHandler NewMessage;
        public ServerHandler()
        {
            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            users = new ObservableCollection<User>();
            this.ConnectToServer();
        }
        private async void ConnectToServer()
        {
            await Task.Run(() =>
            {
                try
                {
                    server.Connect(new IPEndPoint(IPAddress.Parse(this.serverAddress), port));

                    ListenServer();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ConnectToServer : " + ex.Message);
                }
            });
        }
        private async void DisconnectFromServer()
        {
            await Task.Run(() =>
            {
                try
                {
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DisconnectFromServer : " + ex.Message);
                }
            });
        }
        private async void ListenServer()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        byte[] buffer = new byte[256];
                        int bytesRead;
                        string data = new string("");
                        while ((bytesRead = server.Receive(buffer)) > 0)
                        {
                            if (!server.Connected || bytesRead == 0 || server == null) break;
                            data = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                            if (bytesRead < 256)
                            {
                                IncomingMessageHandler(data);
                                break;
                            }
                        }
                        if (!server.Connected || bytesRead == 0 || server == null) break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("dsfds : " + ex.Message);
                    this.DisconnectFromServer();
                }
            });
        }
        private async void IncomingMessageHandler(string response)
        {
            await Task.Run(() =>
            {
                try
                {
                    IncomingMessageHandler(response.FromBase64<MessageInfo>());
                }
                catch (Exception) { }
            });
        }
        private async void IncomingMessageHandler(MessageInfo info)
        {
            await Task.Run(() =>
            {
                try
                {
                    this.NewMessage(info, new EventArgs());
                }
                catch (Exception) { }
            });
        }
        public async void SendMessage(MessageInfo message)
        {
            await Task.Run(() =>
            {
                try
                {
                    string response = message.ToBase64();
                    byte[] buffer = Encoding.Unicode.GetBytes(response);
                    server.Send(buffer);
                }
                catch (Exception ex)
                {
                    this.DisconnectFromServer();
                    MessageBox.Show("SendMessage : " + ex.Message);
                }
            });
        }
    }
}
