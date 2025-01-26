using MiaManager.Services;
using MiaManager.Views;
using MiaManager.Views.Svm;
using MiaManager.Views.User;
using System.Windows;

namespace MiaManager
{
    /// <summary>
    /// Lógica interna para Mia.xaml
    /// </summary>
    public partial class Mia : Window
    {
        UsersMenuView UsersMenuView { get; set; }
        SvmsMenuView SvmsMenuView { get; set; }
        RecognitionMenuView RecognitionMenuView { get; set; }

        public Mia()
        {
            InitializeComponent();
            UserService.Instance.Start();
            SvmService.Instance.Start();
            UsersMenuView = new();
            SvmsMenuView = new();
            RecognitionMenuView = new();
            MainFrame.Navigate(UsersMenuView);

        }

        private void UserPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(UsersMenuView);
        }

        private void SvmPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(SvmsMenuView);
        }

        private void RecognitionPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(RecognitionMenuView);
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainFrame.CanGoBack)
                    MainFrame.GoBack();
            }
            catch
            {

            }

        }

        private void Foward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainFrame.CanGoForward)
                    MainFrame.GoForward();
            }
            catch
            {

            }

        }


    }
}
