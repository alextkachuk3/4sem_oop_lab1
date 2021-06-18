using _4sem_oop_lab1.TCP;
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

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Client.Login(LoginField.Text, PasswordField.Password))
                {
                    MessageBox.Show("Login success!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login fail!");
                }
            }
            catch
            {
                ServerError();
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Client.Register(LoginField.Text, PasswordField.Password))
                {
                    MessageBox.Show("Registration success!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login is busy!");
                }
            }
            catch
            {
                ServerError();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Function shoud run if app can`t connect to server
        /// </summary>
        private void ServerError()
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.WindowState = WindowState.Normal;
            MessageBox.Show("Server doesn't respond!");
            this.Close();
        }
    }
}
