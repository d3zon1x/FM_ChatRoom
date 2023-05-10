using Database;
using ExpansionForCAndS;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelMainWindow
    {
        public string Nick { get; set; }
        public string Message { get; set; }
        public ObservableCollection<MessageInfo> messages { get; set; }
        private RelayCommand SendMsgCommand;

        public IEnumerable<MessageInfo> Messages => messages;
        public ICommand SendMsgCmd => SendMsgCommand;
        public IEnumerable<User> Users => ServerHandler.Instance.users;
        public bool IsConnected => ServerHandler.Instance.IsConnected;
        DatabaseContext databaseContext;
        public ViewModelMainWindow(string nick)
        {
            Nick = nick;
            Message = "";
            messages = new ObservableCollection<MessageInfo>();
            SendMsgCommand = new RelayCommand((a) => SendMessage(), (c) => Message.Length > 0 && IsConnected);
            ServerHandler.Instance.NewMessage += Instance_NewMessage;
            databaseContext =  new DatabaseContext();
            ServerHandler.Instance.SendMessage(new MessageInfo(MessageType.SetNick, Nick));
        }
            
        private async void Instance_NewMessage(object? sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    try
                    {
                        MessageInfo message = sender as MessageInfo;
                        this.IncomingMessageHandler(message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Instance_NewMessage : " + ex.Message);
                    }
                });
            });
        }

        private async void IncomingMessageHandler(MessageInfo info)
        {
            await Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        switch (info.Type)
                        {
                            case MessageType.Private:
                                MessageBox.Show(info.ToString());
                                string temp = (info.From == Nick) ? info.To : info.From;
                                ServerHandler.Instance.users.First(u => u.Nick == temp).messages.Add(info);
                                break;
                            case MessageType.Public:
                                messages.Add(info);
                                break;
                            case MessageType.Update:
                                UpdateUsers(databaseContext.Credential.Select(c=>c.Login).ToList());
                                break;
                            default:
                                break;
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("IncomingMessageHandler : " + ex.Message);
                }
            });
        }

        private async void UpdateUsers(IEnumerable<string> list)
        {
            await Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in list.Where(l=> l!= Nick))
                        {
                            if (!ServerHandler.Instance.users.Select(u => u.Nick).Contains(item))
                            {
                                User user = new User(item, new ObservableCollection<MessageInfo>());
                                ServerHandler.Instance.users.Add(user);
                            }
                        }
                    });
                    
                }
                catch (Exception)
                {

                    throw;
                }
            });
        }
        private async void SendMessage()
        {
            await Task.Run(() => 
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        ServerHandler.Instance.SendMessage(
                                new MessageInfo(MessageType.Public, Nick, "All", Message.ToString())
                            );
                        Message = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SendMessage : " + ex.Message);
                    }
                });
            });
        }
        public async void OpenPrivateChat(string NickTo)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (ServerHandler.Instance.users.Where(x => x.Nick == NickTo).First().IsOpen == true)
                    {
                        return;
                    }
                    PrivateWindow privateWindow = new PrivateWindow(Nick, NickTo);
                    ServerHandler.Instance.users.Where(x => x.Nick == NickTo).First().IsOpen = true;
                    privateWindow.Show();
                    privateWindow.Closed += (s, a) =>
                    {
                        try
                        {
                            ServerHandler.Instance.users.Where(x=>x.Nick == NickTo).First().IsOpen = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message); 
                        }
                    };
                });
            });
        }
    }
}
