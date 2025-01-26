using MiaManager.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiaManager.Views.User
{
    /// <summary>
    /// Interação lógica para UsersMenuView.xam
    /// </summary>
    public partial class UsersMenuView : Page
    {
        public UsersMenuView()
        {
            InitializeComponent();
            UserFrame.Navigate(new UserView());
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UsersMenuViewModel? vm = DataContext as UsersMenuViewModel;
            if (vm != null)
                Console.WriteLine(sender);

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserCreateView userCreateView = new UserCreateView();
            userCreateView.ShowDialog();
        }
    }
}
