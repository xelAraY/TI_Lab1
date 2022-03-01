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
using TI_Lab1.EncryptMethods;
using System.IO;

namespace TI_Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            
            if (MethodCombo.Text == String.Empty)
            {
                MessageBox.Show("Не выбран метод шифрования!");
                return;
            }

            if (MethodCombo.Text == "Столбцовый метод(Ru)" || 
                MethodCombo.Text == "Метод Виженера(Ru)(самогенерирующийся ключ)")
            {
                if (Key_TextBox.Text == String.Empty)
                {
                    MessageBox.Show("Не задан ключ шифрования!");
                    return;
                }

                foreach (char symbol in Key_TextBox.Text)
                {
                    if (!((symbol >= 'А' && symbol <= 'я') || symbol == 'Ё' || symbol == 'ё'))
                    {
                        MessageBox.Show("Введен неверный ключ!");
                        return;
                    }
                }

                switch (btn.Tag.ToString())
                {
                    case "Encode":
                        if (MethodCombo.Text == "Столбцовый метод(Ru)")
                            Decrypt_TextBox.Text = ColumnEncryption.EncryptText(Encrypt_TextBox.Text, Key_TextBox.Text);
                        else
                            Decrypt_TextBox.Text = VigenerEncryption.Encrypt(Encrypt_TextBox.Text, Key_TextBox.Text);
                        break;
                    case "Decode":
                        if (MethodCombo.Text == "Столбцовый метод(Ru)")
                            Decrypt_TextBox.Text = ColumnEncryption.DecryptText(Encrypt_TextBox.Text, Key_TextBox.Text);
                        else
                            Decrypt_TextBox.Text = VigenerEncryption.Decrypt(Encrypt_TextBox.Text, Key_TextBox.Text);
                        break;
                }
            }

            if (MethodCombo.Text == "Метод Плейфера(En)")
            {
                string text = Encrypt_TextBox.Text;
                text = text.ToUpper();
                text = Encryption.ClearText(text, "En");

                if ((text[text.Length-1] == 'X') && (text.Length % 2 != 0))
                {
                    MessageBox.Show("Ошибка в исходном тексте!");
                    return;
                }
                
                for (int i = 1; i < text.Length; i += 2)
                {
                    if (text[i] == 'X' && text[i-1] == 'X')
                    {
                        MessageBox.Show("Ошибка в исходном тексте. Две буквы X подряд");
                        return;
                    }
                }
        
                if (btn.Tag.ToString() == "Encode")
                    Decrypt_TextBox.Text = PlayfairEncryption.Encrypt(Encrypt_TextBox.Text);
                else
                    Decrypt_TextBox.Text = PlayfairEncryption.Decrypt(Encrypt_TextBox.Text);
            }
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        { 
            FileStream fstream = new FileStream("PlainText.txt", FileMode.Open);
            byte[] buffer = new byte[fstream.Length];
            fstream.Read(buffer, 0, buffer.Length);
            string textFromFile = Encoding.Default.GetString(buffer);
            Encrypt_TextBox.Text = textFromFile;
            Button_Click(sender, e);
            fstream.Close();
            fstream = new FileStream("ResultText.txt", FileMode.Create);
            buffer = new byte[Decrypt_TextBox.Text.Length];
            buffer = Encoding.Default.GetBytes(Decrypt_TextBox.Text);
            fstream.Write(buffer, 0, buffer.Length);
            fstream.Close();           
        }
    }   
}
