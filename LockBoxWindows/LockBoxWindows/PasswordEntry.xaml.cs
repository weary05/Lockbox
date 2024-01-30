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
            TextBox confirm = new TextBox();
            confirm.Text = "Confirm password";
            confirm.Margin = new System.Windows.Thickness(0, 40, 0, 0);
            confirm.Height = 20;
            confirm.Width = 100;
            MainGrid.Children.Add(confirm);

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
            TextBox confirm = new TextBox();
            confirm.Text = "Confirm password";
            confirm.Margin = new System.Windows.Thickness(0, 40, 0, 0);
            confirm.Height = 20;
            confirm.Width = 100;
            MainGrid.Children.Add(confirm);

            TextBox oldPass = new TextBox();
            oldPass.Text = "Enter Old Pass";
            oldPass.Margin = new System.Windows.Thickness(0, -40, 0, 0);
            oldPass.Height = 20;
            oldPass.Width = 100;
            MainGrid.Children.Add(oldPass);

            ConfirmButton.Click += new RoutedEventHandler(ChangePasswordConfirm);
                                 
        }
        private void ChangePasswordConfirm(object sender, EventArgs e)
        {
                string oldPassword = oldPass;

                try
                {
                    MainWindow newWindow = new MainWindow(oldPassword);
                    if (confirm == PasswordTextBox.Text)
                    {
                        Password = confirm;

                        string decryptedText = newWindow.Decrypt(newWindow.EncryptedAccountData, oldPassword);
                        MainWindow newPasswordWindow = new MainWindow(Password);
                        string newEncryption = newPasswordWindow.Encrypt(decryptedText, Password);
                        newPasswordWindow.SaveData(newEncryption);
                        newWindow.Close();

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
