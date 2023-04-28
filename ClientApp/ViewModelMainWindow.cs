using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelMainWindow
    {
        public string Nick { get; set; }
        public ViewModelMainWindow(string nick)
        {
            Nick = nick;
        }
    }
}
