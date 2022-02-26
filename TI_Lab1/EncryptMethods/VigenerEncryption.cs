using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_Lab1.EncryptMethods
{
    public static class VigenerEncryption
    {
        const int TABLE_SIZE = 33;
        static char[,] VigenerTable = new char[TABLE_SIZE, TABLE_SIZE];

        public static void FillingVegenerTable(char[,] Table)
        {
            bool check = false;
            for (int i = 0; i < TABLE_SIZE; i++)
            {
                int j = 0;
                int count = 0;
                int count1 = 0;

                if (i == 7 && !check)
                    check = true;

                for (char symbol2 = (char)('А' + i); symbol2 <= 'Я'; symbol2++)
                {
                    if (check && count1 == 0)
                    {
                        symbol2 -= (char)1;
                        count1++;
                    }

                    if (symbol2 == 'Ж' && i != 7 && count == 0)
                    {
                        Table[i, j] = 'Ё';
                        count++;
                        symbol2 -= (char)1;
                    }
                    else
                        Table[i, j] = symbol2;

                    j++;
                }

                if (i == TABLE_SIZE - 1)
                {
                    Table[i, j] = 'Я';
                    j++;
                }

                char symbol3 = 'А';
                count = 0;
                while (j < TABLE_SIZE)
                {
                    if (symbol3 == 'Ж' && count == 0)
                    {
                        Table[i, j] = 'Ё';
                        count++;
                        symbol3 -= (char)1;
                    }
                    else
                        Table[i, j] = symbol3;

                    symbol3++;
                    j++;
                }
            }
        }

        public static string Encrypt(string plaintext, string keyStr)
        {
            string tmpStr = plaintext;
            plaintext = Encryption.ClearText(plaintext, "Ru");
            plaintext = plaintext.ToUpper();
            keyStr = Encryption.ClearText(keyStr, "Ru");
            keyStr = keyStr.ToUpper();

            string encryptedStr = "";
            char[] key;

            if (plaintext.Length > keyStr.Length)
                key = new char[plaintext.Length];
            else
                key = new char[keyStr.Length];

            int i = 0;
            while (i < keyStr.Length)
            {
                key[i] = keyStr[i];
                i++;
            }

            int j = 0;
            while (i < key.Length)
            {
                if (j == plaintext.Length)
                    j = 0;
                key[i] = plaintext[j];
                i++;
                j++;
            }

            FillingVegenerTable(VigenerTable);
            for (int k = 0; k < plaintext.Length; k++)
            {
                (int row, int column) encrSymbol = FindIndex(plaintext[k], key[k]);
                encryptedStr += VigenerTable[encrSymbol.row, encrSymbol.column];
            }

            Encryption.ResultText(tmpStr, ref encryptedStr, "Ru");
            return encryptedStr;
        }

        public static string Decrypt(string encryptedText, string keyStr)
        {
            string tmpStr = encryptedText;
            string resultStr = "";
            encryptedText = Encryption.ClearText(encryptedText, "Ru");
            keyStr = Encryption.ClearText(keyStr, "Ru");
            keyStr = keyStr.ToUpper();

            int i = 0;
            while (i < encryptedText.Length && i < keyStr.Length)
            {
                resultStr += VigenerTable[0, FindRow(keyStr[i], encryptedText[i])];
                i++;
            }

            int j = 0;
            while (i < encryptedText.Length)
            {
                resultStr += VigenerTable[0, FindRow(resultStr[j], encryptedText[i])];
                i++;
                j++;
            }

            Encryption.ResultText(tmpStr, ref resultStr, "Ru");
            return resultStr;
        }

        public static (int, int) FindIndex(char symbol1, char symbol2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < TABLE_SIZE; i++)
            {
                if (symbol1 == VigenerTable[0, i])
                    index1 = i;
                if (symbol2 == VigenerTable[0, i])
                    index2 = i;
            }
            return (index1, index2);
        }

        public static int FindRow(char symbol1, char symbol2)
        {
            int index1 = 0;
            for (int i = 0; i < TABLE_SIZE; ++i)
            {
                if (symbol1 == VigenerTable[i, 0])
                    index1 = i;
            }

            for (int i = 0; i < TABLE_SIZE; i++)
            {
                if (symbol2 == VigenerTable[index1, i])
                    return i;
            }
            return -1;
        }
    }
}
