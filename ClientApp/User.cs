using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpansionForCAndS
{
    [AddINotifyPropertyChangedInterface]
    public class User
    {
        public string Nick { get; set; }
        public bool IsOpen { get; set; }
        public ObservableCollection<MessageInfo> messages { get; set; }
        public IEnumerable<MessageInfo> Messages => messages;
        public User(string nick, ObservableCollection<MessageInfo> messages)
        {
            Nick = nick; 
            this.messages = messages;
            IsOpen = false;
        }

        public override string ToString()
        {
            return $"{Nick}";
        }
    }
}
