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

namespace LockBoxWindows
{
    /// <summary>
    /// Interaction logic for PasswordEntry.xaml
    /// </summary>
    public partial class PasswordEntry : Window
    {
        public string Password = string.Empty;
        public PasswordEntry()
        {
            InitializeComponent();
        }

        private void SubmitPassword(object sender, RoutedEventArgs e)
        {
            Password = PasswordTextBox.Text;
            try
            {
                MainWindow newWindow = new MainWindow(Password);
                newWindow.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("The Password Entered Is Incorrect");
            }
        }
        private void CreateNewPassword(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = "Enter New Password";
            confirm.Visibility= Visibility.Visible;

            Button confirmButton = new Button();
            confirmButton.Height = 20;
            confirmButton.Width = 20;
            confirmButton.Content = "->";
            confirmButton.Margin = new Thickness(130, 0, 0, 0);
            MainGrid.Children.Add(confirmButton);
            MainGrid.Children.Remove(ConfirmButton);
            confirmButton.Click += new RoutedEventHandler(ConfirmCreatePassword);

        }
        private void ConfirmCreatePassword(object sender, RoutedEventArgs e)
        {
            if (confirm.Text == PasswordTextBox.Text)
            {
                Password = confirm.Text;

                MainWindow newWindow = new MainWindow(Password);
                newWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }
        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = "Enter New Password";
            oldPass.Visibility = Visibility.Visible;
            confirm.Visibility = Visibility.Visible;

            Button confirmButton = new Button();
            confirmButton.Height= 20;
            confirmButton.Width= 20;
            confirmButton.Content = "->";
            confirmButton.Margin = new Thickness(130,0, 0, 0);
            
            MainGrid.Children.Add(confirmButton);
            MainGrid.Children.Remove(ConfirmButton);
            confirmButton.Click += new RoutedEventHandler(ChangePasswordConfirm);
                                 
        }
        private void ChangePasswordConfirm(object sender, EventArgs e)
        {
                string oldPassword = oldPass.Text;

                try
                {
                    MainWindow newWindow = new MainWindow(oldPassword);
                    if (confirm.Text == PasswordTextBox.Text)
                    {
                        Password = confirm.Text;

                        string decryptedText = newWindow.Decrypt(newWindow.EncryptedAccountData, oldPassword);
                        
                        string newEncryption = newWindow.Encrypt(decryptedText, Password);
                        newWindow.SaveData(newEncryption);
                        newWindow.Close();
                        MainWindow newPasswordWindow = new MainWindow(Password);

                        newPasswordWindow.Show();
                        this.Close();
                    }
                }

                catch
                {
                    MessageBox.Show("The Original Password Entered Is Incorrect");
                }            
        }
    }
}
