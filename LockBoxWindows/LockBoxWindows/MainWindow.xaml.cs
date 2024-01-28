using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
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
    
    public partial class MainWindow : Window
    {
        bool editing = false;
        List<Account> accounts = new List<Account>();
        const string defaultAccount = "Welcome to Lockbox";
        const string defaultPassword = "Store Your Passwords Here";
        const string defaultNotes = "Press on new to register a new account\r\nSelect edit to change details for an existing account\r\nSelect delete to remove the currently selected account";

        string EncryptedAccountData = string.Empty;
        string DecryptedAccountData = string.Empty;
        string Password = "password";
        

        public MainWindow()
        {
            InitializeComponent();
            EncryptedAccountData = RetrieveData();            
            //GetPasswordFromUser
            DecryptedAccountData = Decrypt();
            ConvertStringToAccounts();
            RefreshList();
        }

        private void ConvertStringToAccounts()
        {
            string[] stringAccounts = DecryptedAccountData.Split(';');
            foreach(string accountData in stringAccounts)
            {
                if (accountData != string.Empty)
                {
                    string[] data = accountData.Split(',');
                    Account newAccount = new Account(data[0], data[1], data[2]); 
                    accounts.Add(newAccount);
                }
            }
        }

        private string Decrypt()
        {
            string encrypted = EncryptedAccountData;
            byte[] textbytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = Encoding.UTF8.GetBytes("1a1a1a1a1a1a1a1a");
            endec.Key = sha256_hash(Password);
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(enc);
        }
        private string Encrypt() 
        {
            string decrypted = DecryptedAccountData;
            byte[] textbytes = ASCIIEncoding.ASCII.GetBytes(decrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = Encoding.UTF8.GetBytes("1a1a1a1a1a1a1a1a");
            endec.Key = sha256_hash(Password);
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return Convert.ToBase64String(enc);
        }
        
        private Byte[] sha256_hash(String value)//https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertDataToString();
            EncryptedAccountData = Encrypt();

            SaveData(EncryptedAccountData);

            this.Close();
        }

        private void ConvertDataToString()
        {
            string temp = string.Empty;
            foreach(Account account in accounts)
            {
                string accountString = $"{account.accountName},{account.password},{account.extraNotes};";
                temp += accountString;
            }
            DecryptedAccountData = temp;
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
                SaveButton.Visibility = Visibility.Hidden;
                SaveButton.IsEnabled = false;


            }
            else
            {                
                AccountNameBox.IsReadOnly = false;
                PasswordEntryBox.IsReadOnly = false;
                ExtraNotesBox.IsReadOnly = false;
                EditModeButton.Content = "Editing";
                EditModeButton.Foreground = Brushes.Red;
                editing= true;

                SaveButton.Visibility = Visibility.Visible; 
                SaveButton.IsEnabled = true;

            }

        }
        private void SaveAccountData(object sender, RoutedEventArgs e) 
        {
            string name = AccountNameBox.Text, password = PasswordEntryBox.Text, notes = ExtraNotesBox.Text;

            Account saveAccount = new Account(name, password, notes);
            
            int i = 0;
            try
            {
                do
                {
                    if (accounts[i].accountName == name)
                    {
                        accounts.Remove(accounts[i]);
                    }
                    i++;
                } while (!accounts[i - 1].Equals(accounts.Last()));
            }
            catch(System.ArgumentOutOfRangeException)
            {
            }

            accounts.Add(saveAccount);

            editing = true;
            EditModeToggle(sender,e);

            RefreshList();
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
            editing = true;
            EditModeToggle(sender, e);

            string name = AccountNameBox.Text;

            foreach (Account account in accounts)
            {
                if (account.accountName == name)
                {
                    accounts.Remove(account);
                    AccountNameBox.Text = defaultAccount;
                    PasswordEntryBox.Text = defaultPassword;
                    ExtraNotesBox.Text = defaultNotes;
                    RefreshList();
                    break;
                }
            }

        }
        private void RefreshList()
        {
            AccountList.Children.Clear();
            foreach(Account account in accounts)
            {
                Button button = new Button();
                button.Content = account.accountName;
                button.Foreground = Brushes.White;
                button.Background = Brushes.Transparent;
                button.Click += new RoutedEventHandler(OpenAccountData);
                AccountList.Children.Add(button);
                
            }
        }
        private void OpenAccountData(object sender, RoutedEventArgs e)
        {
            editing = true;
            EditModeToggle(sender, e);

            string name = sender.ToString();
            name = name.Substring(name.IndexOf(" ") + 1);
            
            foreach(Account account in accounts)
            {
                if (account.accountName == name)
                {
                    AccountNameBox.Text = account.accountName;
                    PasswordEntryBox.Text = account.password;
                    ExtraNotesBox.Text = account.extraNotes;
                }
            }
        }
        public string RetrieveData()
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Data.txt";
            StreamReader reader = new StreamReader(fileName);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
        public void SaveData(string data)
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Data.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(data);
            writer.Close();
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
