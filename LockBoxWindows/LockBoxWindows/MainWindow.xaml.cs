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

namespace LockBoxWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool editing = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void EditModeToggle(object sender, RoutedEventArgs e)
        {
            if (editing)
            {
                AccountNameBox.IsReadOnly = true;
                PasswordEntryBox.IsReadOnly = true;
                ExtraNotesBox.IsReadOnly = true;
                EditModeButton.Content = "Edit";
                EditModeButton.Foreground = Brushes.White;
                editing = false;

                string name = string.Empty, password = string.Empty, notes = string.Empty;

                Account saveAccount = new Account(name, password, notes);

            }
            else
            {                
                AccountNameBox.IsReadOnly = false;
                PasswordEntryBox.IsReadOnly = false;
                ExtraNotesBox.IsReadOnly = false;
                EditModeButton.Content = "Editing";
                EditModeButton.Foreground = Brushes.Red;
                editing= true;
            }

        }
        private void CreateNewEntry(object sender, RoutedEventArgs e)
        {
            editing = false;
            EditModeToggle(sender, e);

            AccountNameBox.Text = "Name";
            PasswordEntryBox.Text = "Password";
            ExtraNotesBox.Text = "Notes";
                       
        }
        private void DeleteCurrent(object sender, RoutedEventArgs e)
        {
            
        }
        private void RefreshList()
        {

        }
    }
    struct Account
    {
        public string accountName;
        public string password;
        public string extraNotes;

        public Account(string inAccountName, string inPassword, string inNotes)
        {
            accountName = inAccountName;
            password = inPassword;
            extraNotes = inNotes;
        }
    }
}
