using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApp
{
    public partial class LoginWindow : Window
    {
        ViewModelLogin viewModelLogin;
        public LoginWindow()
        {
            InitializeComponent();
            viewModelLogin = new ViewModelLogin();
            viewModelLogin.OnRequestClose += (s, e) => this.Close();
            this.DataContext = viewModelLogin;
        }
    }
}
