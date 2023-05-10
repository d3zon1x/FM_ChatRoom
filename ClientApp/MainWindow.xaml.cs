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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModelMainWindow model;
        public MainWindow(string nick)
        {
            InitializeComponent();
            model = new ViewModelMainWindow(nick);
            this.DataContext = model;
        }


        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            var selectedItem = listBox.SelectedItem;
            if (selectedItem != null)
            {
                var clickedItem = selectedItem.ToString();
                model.OpenPrivateChat(clickedItem);
            }
        }
    }
}
