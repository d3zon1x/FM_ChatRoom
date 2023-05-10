using ExpansionForCAndS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole
{
    public class UserHendler
    {
        private Socket socket;
        public string Nick { get; set; }
        static int _count = 0;

        public UserHendler(Socket socket, string nick)
        {
            this.socket = socket;
            Nick = nick;
            ListenClient();
        }

        public UserHendler(Socket socket): this(socket, $"unknown{++_count}") { }

        private async void ListenClient()
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
                        while ((bytesRead = socket.Receive(buffer)) > 0)
                        {
                            if (!socket.Connected || bytesRead == 0 || socket == null) break;
                            data = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                            if (bytesRead < 256)
                            {
                                Console.WriteLine(data);
                                MessageInfo message = data.FromBase64<MessageInfo>();
                                IncomingMessageHandler(message);
                                break;
                            }
                        }
                        if (!socket.Connected || bytesRead == 0 || socket == null) break;
                      
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Socket Exception: {0}", ex.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ListenClient : " + ex.Message);
                }
                finally
                {
                    Disconnect();
                    Console.WriteLine("Close");
                }
            });
        }

        private async void Disconnect()
        {
            await Task.Run(() =>
            {
                try
                {
                    ChatServer.Instance.RemoveUser(Nick);
                    ChatServer.Instance.SendMessageToAll(new MessageInfo(MessageType.Public, Nick, "All", "Disconnect from server"));
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    Console.WriteLine("Close");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Disconnect : " + ex.Message);
                }
            });
        }

        private async void IncomingMessageHandler(MessageInfo data)
        {
            await Task.Run(() =>
            {
                ChatServer.Instance.db.LogMassageInfos.Add(new Database.Entities.LogMassageInfo()
                {
                    Type = data.Type.ToString(),
                    From = data.From,
                    To = data.To,
                    Message = data.Message,
                });
                ChatServer.Instance.db.SaveChangesAsync();
                // logs message
                Console.WriteLine(data);
                switch (data.Type)
                {
                    case MessageType.Disconnect:
                        //this.tcpClient.Close();
                        break;
                    default:
                        ChatServer.Instance.IncomingMessageHandlerOnServer(data);
                        break;
                }
            });
        }

       

        public async void SendMessage(MessageInfo messageInfo)
        {
            await Task.Run(() =>
            {
                try
                {
                    string response = messageInfo.ToBase64();
                    byte[] buffer = Encoding.Unicode.GetBytes(response);
                    socket.Send(buffer);
                }
                catch (Exception ex)
                {
                    Disconnect();
                    Console.WriteLine("SendMessage : " + ex.Message);
                }
            });
        }

        public override string ToString()
        {
            return $"Nick {Nick}, {socket.LocalEndPoint}, {socket?.Connected}";
        }
    }
}
