using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelLogin
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValidLogin => Login.Length > 6 && Login.Length < 20;
        public bool IsValidPassword => Password.Length > 6 && Password.Length < 20;
        public bool IsValidEmail => new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(Email).Success;
        private RelayCommand LoginCommand;
        private RelayCommand RegisterCommand;
        public ICommand LoginCmd => LoginCommand;
        public ICommand RegisterCmd => RegisterCommand;
        public ViewModelLogin()
        {
            Login = "";
            Email = "";
            Password = "";
            LoginCommand = new RelayCommand((o) => LoginBtnClick(), (o) => IsValidLogin && IsValidPassword);
            RegisterCommand = new RelayCommand((o) => RegisterBtnClick(), (o) => IsValidLogin && IsValidPassword && IsValidEmail);
        }
        private void LoginBtnClick()
        {

        }
        private void RegisterBtnClick()
        {

        }
    }
}
