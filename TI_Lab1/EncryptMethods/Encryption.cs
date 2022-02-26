using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_Lab1.EncryptMethods
{
    public static class Encryption
    {
        public delegate bool IsCorrectSymbol(char symbol);
        public static bool IsCorrectSymbolRu(char symbol)
        {
            if (symbol == 'ё' || symbol == 'Ё')
                return true;

            for (char c = 'А'; c <= 'я'; c++)
            {
                if (c == symbol)
                    return true;
            }
            return false;
        }

        public static bool IsCorrectSymbolEn(char symbol)
        {
            for (char c = 'A'; c <= 'z'; c++)
            {
                if (c == symbol)
                    return true;
            }
            return false;
        }

        public static string ClearText(string plaintext, string language)
        {
            string correctText = "";
            IsCorrectSymbol isCorrectSymbol = GetCorrectSymbolMethod(language);

            for (int i = 0; i < plaintext.Length; i++)
            {   
                if (isCorrectSymbol(plaintext[i]))
                    correctText += plaintext[i];
            }
            return correctText;
        }
        public static void ResultText(string plaintext, ref string encryptedText, string language)
        {
            IsCorrectSymbol isCorrectSymbol = GetCorrectSymbolMethod(language);

            string tmpStr = "";
            for (int i = 0; i < plaintext.Length; i++)
            {
                if (!isCorrectSymbol(plaintext[i]))
                {
                    tmpStr = "";
                    tmpStr += plaintext[i];
                    if (i < encryptedText.Length)
                        encryptedText = encryptedText.Insert(i, tmpStr);
                    else
                        encryptedText = string.Concat(encryptedText, tmpStr);
                }
            }
        }

        public static IsCorrectSymbol GetCorrectSymbolMethod(string language)
        {
            if (language == "Ru")
                return IsCorrectSymbolRu;
            else
                return IsCorrectSymbolEn;
        }
    }
}
