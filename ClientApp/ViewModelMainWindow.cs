using ExpansionForCAndS;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelMainWindow
    {
        public string Nick { get; set; }
        public string Message { get; set; }
        public ObservableCollection<MessageInfo> messages { get; set; }
        public IEnumerable<MessageInfo> Messages => messages;
        private RelayCommand SendMsgCommand;
        public ICommand SendMsgCmd => SendMsgCommand;
        public IEnumerable<User> Users => ServerHandler.Instance.users;
        public ViewModelMainWindow(string nick)
        {
            Nick = nick;
            Message = "";
            messages = new ObservableCollection<MessageInfo>();
            SendMsgCommand = new RelayCommand((a) => SendMessage(), (c) => Message.Length > 0);
        }
        private async void SendMessage()
        {
            await Task.Run(() => 
            {
                Message = "";
            });
        }
    }
}
