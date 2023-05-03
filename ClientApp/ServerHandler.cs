using ExpansionForCAndS;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ServerHandler : SingletonClass<ServerHandler>
    {
        public ObservableCollection<User> users;
        public ServerHandler()
        {
            users = new ObservableCollection<User>();
        }
    }
}
