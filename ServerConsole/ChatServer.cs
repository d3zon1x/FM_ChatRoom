using Database;
using ExpansionForCAndS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerConsole
{
    public class ChatServer: SingletonClass<ChatServer>
    {
        private string serverAddress = "127.0.0.1";
        private int port = 4040;
        Socket server;
        private int MaxUsers = 10;
        private List<UserHendler> users;
        public DatabaseContext db;

        public ChatServer()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(serverAddress), port));
            users = new List<UserHendler>();
            db = new DatabaseContext();
        }

        #region SendMessages
        public async void SendMessageToAll(MessageInfo message)
        {
            await Task.Run(() =>
            {
                Console.WriteLine(message);
                foreach (UserHendler user in this.users)
                {
                    try
                    {
                        user.SendMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SendMessageToAll" + ex.Message);
                    }
                }
            });
        }
        private async void SendToSpecificUser(MessageInfo message, string nickTo)
        {
            await Task.Run(() =>
            {
                try
                {
                    //users.Where(i => i.Nick == nickTo).First().SendMessage(message);
                    foreach (UserHendler item in users.Where(i => i.Nick == nickTo))
                    {
                        item.SendMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SendToSpecificUser : " + ex.Message);
                }
            });
        }
        #endregion

        #region UserConnectAndDisconnect
        public bool IsNickUnique(string nick) => !users.Select(i => i.Nick).Contains(nick);
        public async void RemoveUser(string Nick)
        {
            await Task.Run(() =>
            {
                this.users.RemoveAll(i => i.Nick == Nick);
            });
        }
        #endregion

        public async void Start()
        {
            await Task.Run(() =>
            {
                
                try
                {
                    server.Listen(MaxUsers);
                    Console.WriteLine($"Server started {serverAddress}:{port}");

                    while (true)
                    {
                        // Очікуємо на підключення клієнта та створюємо сокет для роботи з ним
                        Socket client = server.Accept();

                        Console.WriteLine($"Client connected from {((IPEndPoint)client.RemoteEndPoint).Address}");

                        // Створюємо новий потік для обробки з'єднання з клієнтом
                        System.Threading.ThreadPool.QueueUserWorkItem(HandleClientAsync, client);
                    }
                }
                catch (Exception ex)
                {
                    // Виводимо повідомлення про помилку в консоль
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    // Закриваємо сокет та закриваємо всі з'єднання
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    Console.WriteLine($"Server stopped on port {port}");
                }
            });
        }

        private async void HandleClientAsync(object obj)
        {
            await Task.Run(() =>
            {

                // Отримуємо сокет клієнта з параметрів методу
                Socket client = obj as Socket;

                //taska

                UserHendler user = new UserHendler(client);
                if (MaxUsers <= users.Count())
                {
                    user.SendMessage(new MessageInfo(MessageType.Exception, "You", "Server", "The server is full"));
                }
                else
                {
                    Console.WriteLine($"Client connected");
                    this.users.Add(user);
                    Console.WriteLine(user);
                }
            });
        }
        #region HandlersMessages
        public async void IncomingMessageHandlerOnServer(byte[] data)
        {
            await Task.Run(() =>
            {
                this.IncomingMessageHandlerOnServer(Encoding.Unicode.GetString(data).FromBase64<MessageInfo>());
            });
        }

        public async void IncomingMessageHandlerOnServer(MessageInfo message)
        {
            await Task.Run(() =>
            {
                switch (message.Type)
                {
                    case MessageType.Public:
                        SendMessageToAll(message);
                        break;
                    case MessageType.Private:
                        SendToSpecificUser(message, message.To);
                        break;
                    case MessageType.Exception:
                        break;
                    default:
                        break;
                }
            });
        }
        #endregion

    }
}
