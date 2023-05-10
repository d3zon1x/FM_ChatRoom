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
    public class ViewModelPrivate
    {
        public string Nick { get; set; }
        public string NickTo { get; set; }
        public string Message { get; set; }
        private ObservableCollection<MessageInfo> messages => ServerHandler.Instance.users.Where(x => x.Nick == NickTo).First().messages;
        public IEnumerable<MessageInfo> Massages => messages;

        private RelayCommand SendMsgCommand;
        public ICommand SendCmd => SendMsgCommand;
        public ViewModelPrivate(string nick, string nickTo)
        {
            Nick = nick;
            NickTo = nickTo;
            Message = "";
            SendMsgCommand = new RelayCommand((a) => SendMessage(), (c) => Message.Length > 0   ); 
        }

        public async void SendMessage() 
        {
            await Task.Run(() => 
            {
                ServerHandler.Instance.SendMessage(new MessageInfo(MessageType.Private, Nick, NickTo, Message)); 
                Message = "";
            });
        }
    }
}
