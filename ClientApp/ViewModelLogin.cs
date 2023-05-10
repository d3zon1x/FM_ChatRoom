using Database;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClientApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelLogin
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValidLogin => Login.Length > 3 && Login.Length < 20;
        public bool IsValidPassword => Password.Length > 3 && Password.Length < 20;
        public bool IsValidEmail => new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(Email).Success;
        private RelayCommand LoginCommand;
        private RelayCommand RegisterCommand;
        public ICommand LoginCmd => LoginCommand;
        public ICommand RegisterCmd => RegisterCommand;
        private DatabaseContext db = null;
        public event EventHandler OnRequestClose;
        public ViewModelLogin()
        {

            Login = "Test";
            Email = "";
            Password = "test";
            LoginCommand = new RelayCommand((o) => LoginBtnClick(), (o) => IsValidLogin && IsValidPassword);
            RegisterCommand = new RelayCommand((o) => RegisterBtnClick(), (o) => IsValidLogin && IsValidPassword && IsValidEmail);
            db = new DatabaseContext();
            dbInit();
        }
        private async void dbInit()
        {
            await Task.Run(() =>
            {
                try
                {
                    db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        private async void LoginBtnClick()
        {
            await Task.Run(() =>
            {
                //MessageBox.Show(Login + Password);
                try
                {
                    var user = db.Credential.Where(x => x.Login == Login && x.Password == Password).First();
                    if (user == null)
                    {
                        MessageBox.Show("User not found!");
                    }
                    else
                    {
                        OpenMainWindow();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        private async void RegisterBtnClick()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (db.Credential.Any(x => x.Login == Login))
                    {
                        MessageBox.Show($"login {Login} has already taken!");
                    }
                    else
                    {
                        db.Credential.Add(new Database.Entities.Credentials()
                        {
                            Login = this.Login,
                            Password = this.Password,
                            Email = this.Email,
                            LastVisit = DateTime.Now
                        });
                        db.SaveChangesAsync();
                        OpenMainWindow();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });      
        }
        private async void OpenMainWindow()
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow mainWindow = new MainWindow(Login);
                    mainWindow.Show();
                    OnRequestClose(this, new EventArgs());
                });
            });
        }
    }
}
