using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_Lab1.EncryptMethods
{
    public static class PlayfairEncryption
    {
        const int CIPHER_TABLE_SIZE = 5;
        static char[,] CIPHER_TABLE = { { 'C', 'R', 'Y', 'P', 'T' },
                                        { 'O', 'G', 'A', 'H', 'B' },
                                        { 'D', 'E', 'F', 'I', 'K' },
                                        { 'L', 'M', 'N', 'Q', 'S' },
                                        { 'U', 'V', 'W', 'X', 'Z' }
        };

        public static char Symbol(char symbol)
        {
            if (symbol == 'J')
                return 'I';
            else
                return symbol;
        }

        public static string Encrypt(string plaintext)
        {
            Encryption.IsCorrectSymbol isCorrectSymbol = Encryption.GetCorrectSymbolMethod("En");
            string tmpStr = plaintext;
            plaintext = Encryption.ClearText(plaintext, "En");
            plaintext = plaintext.ToUpper();
            string resultStr = "";
            string nullSymbol = "X";
            int i = 0;
            int j = 0;
            bool isConcat = false;
            bool isInsert = false;

            while (i < plaintext.Length)
            {
                (int row, int column) firstSymbol = FindIndex(Symbol(plaintext[i]));
                i++;

                while (!isCorrectSymbol(tmpStr[j]))
                    j++;
                j++;

                isConcat = false;
                if (i >= plaintext.Length)
                {
                    plaintext = string.Concat(plaintext, nullSymbol);
                    tmpStr = tmpStr.Insert(j, nullSymbol);
                    isConcat = true;
                }

                isInsert = false;
                if (plaintext[i] == plaintext[i - 1] && !isConcat)
                {
                    plaintext = plaintext.Insert(i, nullSymbol);
                    tmpStr = tmpStr.Insert(j, nullSymbol);
                    isInsert = true;
                    j++;
                }

                (int row, int column) secondSymbol = FindIndex(Symbol(plaintext[i]));

                if (firstSymbol.row == secondSymbol.row)
                {
                    if (firstSymbol.column == CIPHER_TABLE_SIZE - 1)
                        firstSymbol.column = 0;
                    else
                        firstSymbol.column++;

                    if (secondSymbol.column == CIPHER_TABLE_SIZE - 1)
                        secondSymbol.column = 0;
                    else
                        secondSymbol.column++;
                }

                if (firstSymbol.column == secondSymbol.column && plaintext[i]!=plaintext[i-1])
                {
                    if (firstSymbol.row == CIPHER_TABLE_SIZE - 1)
                        firstSymbol.row = 0;
                    else
                        firstSymbol.row++;

                    if (secondSymbol.row == CIPHER_TABLE_SIZE - 1)
                        secondSymbol.row = 0;
                    else
                        secondSymbol.row++;
                }

                if ((firstSymbol.row != secondSymbol.row) &&
                    (firstSymbol.column != secondSymbol.column))
                {
                    int difference = firstSymbol.column - secondSymbol.column;
                    if (difference > 0)
                    {
                        firstSymbol.column -= difference;
                        secondSymbol.column += difference;
                    }
                    else
                    {
                        difference = -difference;
                        firstSymbol.column += difference;
                        secondSymbol.column -= difference;
                    }
                }
                resultStr += CIPHER_TABLE[firstSymbol.row, firstSymbol.column];
                resultStr += CIPHER_TABLE[secondSymbol.row, secondSymbol.column];
                i++;
                if (!isInsert)
                    j++;
            }

            Encryption.ResultText(tmpStr, ref resultStr, "En");
            return resultStr;
        }

        public static string Decrypt(string encryptedText)
        {

            Encryption.IsCorrectSymbol isCorrectSymbol = Encryption.GetCorrectSymbolMethod("En");
            string tmpStr = encryptedText;
            encryptedText = Encryption.ClearText(encryptedText, "En");
            encryptedText = encryptedText.ToUpper();
            string nullSymbol = "X";
            int i = 0;
            int j = 0;
            bool isConcat = false;         
            string resultStr = "";


            while (i < encryptedText.Length)
            {
                (int row, int column) firstSymbol = FindIndex(Symbol(encryptedText[i]));
                i++;

                while (!isCorrectSymbol(tmpStr[j]))
                    j++;
                j++;

                isConcat = false;
                if (i >= encryptedText.Length)
                {
                    encryptedText = string.Concat(encryptedText, nullSymbol);
                    tmpStr = tmpStr.Insert(j, nullSymbol);
                    isConcat = true;
                }

                if (encryptedText[i] == encryptedText[i - 1] && !isConcat)
                {
                    encryptedText = encryptedText.Insert(i, nullSymbol);
                    tmpStr = tmpStr.Insert(j, nullSymbol);
                    j++;
                }

                (int row, int column) secondSymbol = FindIndex(Symbol(encryptedText[i]));

                if (firstSymbol.row == secondSymbol.row)
                {
                    if (firstSymbol.column == 0)
                        firstSymbol.column = CIPHER_TABLE_SIZE - 1;
                    else
                        firstSymbol.column--;

                    if (secondSymbol.column == 0)
                        secondSymbol.column = CIPHER_TABLE_SIZE - 1;
                    else
                        secondSymbol.column--;
                }

                if (firstSymbol.column == secondSymbol.column && encryptedText[i] != encryptedText[i - 1])
                {
                    if (firstSymbol.row == 0)
                        firstSymbol.row = CIPHER_TABLE_SIZE - 1;
                    else
                        firstSymbol.row--;

                    if (secondSymbol.row == 0)
                        secondSymbol.row = CIPHER_TABLE_SIZE - 1;
                    else
                        secondSymbol.row--;
                }

                if ((firstSymbol.row != secondSymbol.row) &&
                    (firstSymbol.column != secondSymbol.column))
                {
                    int difference = firstSymbol.column - secondSymbol.column;
                    if (difference > 0)
                    {
                        firstSymbol.column -= difference;
                        secondSymbol.column += difference;
                    }
                    else
                    {
                        difference = -difference;
                        firstSymbol.column += difference;
                        secondSymbol.column -= difference;
                    }
                }
                resultStr += CIPHER_TABLE[firstSymbol.row, firstSymbol.column];
                resultStr += CIPHER_TABLE[secondSymbol.row, secondSymbol.column];
                i++;
            }

            Encryption.ResultText(tmpStr, ref resultStr, "En");
            return resultStr;
        }

        public static (int, int) FindIndex(char symbol)
        {
            for (int i = 0; i < CIPHER_TABLE_SIZE; i++)
            {
                for (int j = 0; j < CIPHER_TABLE_SIZE; j++)
                {
                    if (CIPHER_TABLE[i, j] == symbol)
                    {
                        var index = (i, j);
                        return index;
                    }
                }
            }
            return (-1, -1);
        }
    }
}