using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace ServerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Встановлюємо порт сервера та створюємо сокет для прийому з'єднань
            int port = 12345;
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Прив'язуємо сокет до локальної адреси та порту
                server.Bind(new IPEndPoint(IPAddress.Any, port));

                // Починаємо прослуховування порту
                server.Listen(100);

                Console.WriteLine($"Server started on port {port}");

                // Безкінечний цикл для прийому та обробки з'єднань
                while (true)
                {
                    // Очікуємо на підключення клієнта та створюємо сокет для роботи з ним
                    Socket client = server.Accept();

                    Console.WriteLine($"Client connected from {((IPEndPoint)client.RemoteEndPoint).Address}");

                    // Створюємо новий потік для обробки з'єднання з клієнтом
                    System.Threading.ThreadPool.QueueUserWorkItem(HandleClient, client);
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
                server.Close();
                Console.WriteLine($"Server stopped on port {port}");
            }
        }



        static void HandleClient(object obj)
        {
            // Отримуємо сокет клієнта з параметрів методу
            Socket client = obj as Socket;

            try
            {
                // Безкінечний цикл для читання даних від клієнта та відправки відповідей
                while (true)
                {
                    // Отримуємо дані від клієнта
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);

                    // Якщо клієнт відключився, вихід з циклу
                    if (bytesRead == 0)
                        break;

                    // Перетворюємо дані на рядок та виводимо на екран
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: {0}", data);

                    // Відправляємо відповідь клієнту
                    byte[] response = Encoding.UTF8.GetBytes($"Server response {data}");
                    client.Send(response);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket Exception: {0}", ex.Message);
            }
            finally
            {
                // Закриваємо з'єднання з клієнтом
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                Console.WriteLine("Close");
            }
        }

    }
}
