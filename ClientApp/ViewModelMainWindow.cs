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
        public ViewModelMainWindow(string nick)
        {
            Nick = nick;
            Message = "";
            messages = new ObservableCollection<MessageInfo>();
            SendMsgCommand = new RelayCommand((a) => SendMessage(), (c) => Message.Length > 0 && IsConnected);
            ServerHandler.Instance.NewMessage += Instance_NewMessage;
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
                        messages.Add(message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Instance_NewMessage : " + ex.Message);
                    }
                });
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
    }
}
