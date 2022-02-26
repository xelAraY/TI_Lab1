using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_Lab1.EncryptMethods
{
    public static class ColumnEncryption
    { 
        public static string EncryptText(string plaintext, string key)
        {
            string tmpStr = plaintext;
            plaintext = Encryption.ClearText(plaintext, "Ru");
            key = Encryption.ClearText(key, "Ru");
            string chipretext = "";
            bool check = false;

            for (char symbol = 'А'; symbol <= 'я'; symbol++)
            {
                char currentSymbol = symbol;
                int j = 0;

                if (currentSymbol == 'Ж' && !check)
                    symbol = 'Ё';
                else if (currentSymbol == 'ж' && !check)
                    symbol = 'ё';

                while (j < key.Length)
                {
                    if (key[j] == symbol)
                    {
                        int k = j;
                        while (k < plaintext.Length)
                        {
                            chipretext += plaintext[k];
                            k += key.Length;
                        }
                    }
                    j++;
                }
                if (symbol == 'Ё' || symbol == 'ё')
                {
                    symbol = (char)(currentSymbol - 1);
                    check = true;
                }
            }
            Encryption.ResultText(tmpStr, ref chipretext, "Ru");
            return chipretext;
        }

        public static string DecryptText(string encryptedText, string key)
        {
            string tmpStr = encryptedText;
            encryptedText = Encryption.ClearText(encryptedText, "Ru");
            key = Encryption.ClearText(key, "Ru");
            char[] tmpText = new char[encryptedText.Length];
            bool check = false;
            int indexCount;
            int index = 0;

            for (char symbol = 'А'; symbol <= 'я'; symbol++)
            {
                char currentSymbol = symbol;
                int j = 0;

                if (currentSymbol == 'Ж' && !check)
                    symbol = 'Ё';
                else if (currentSymbol == 'ж' && !check)
                    symbol = 'ё';

                while (j < key.Length)
                {
                    if (key[j] == symbol)
                    {
                        int count = 0;
                        indexCount = encryptedText.Length - 1 - j - count * key.Length;
                        while (indexCount >= 0)
                        {
                            tmpText[j + count * key.Length] = encryptedText[index];
                            index++;
                            count++;
                            indexCount = encryptedText.Length - 1 - j - count * key.Length;
                        }
                    }
                    j++;
                }
                if (symbol == 'Ё' || symbol == 'ё')
                {
                    symbol = (char)(currentSymbol - 1);
                    check = true;
                }
            }

            string decryptedStr = "";
            for (int i = 0; i < encryptedText.Length; i++)
            {
                decryptedStr += tmpText[i];
            }

            Encryption.ResultText(tmpStr, ref decryptedStr, "Ru");
            return decryptedStr;
        }
    }   
}
