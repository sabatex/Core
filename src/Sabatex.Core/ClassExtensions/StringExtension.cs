using System.Text;
using System.Collections.Specialized;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace Sabatex.Core.ClassExtensions;
/// <summary>
/// Provides extension methods for string manipulation, parsing, validation, transliteration, and conversion operations.
/// </summary>
/// <remarks>The StringExtension class includes a variety of static extension methods for the string type,
/// enabling common operations such as parsing substrings, validating character sets, converting between Unicode
/// representations, and transliterating between Cyrillic and Latin scripts. These methods are designed to simplify
/// string handling in scenarios involving custom delimiters, language-specific conversions, and numeric parsing. All
/// methods are implemented as extension methods and can be called directly on string instances.</remarks>
public static class StringExtension
{
    /// <summary>
    /// Extracts the substring from the start of the input string up to the first occurrence of the specified delimiter,
    /// trimming any leading or trailing whitespace.
    /// </summary>
    /// <remarks>If the delimiter is not found in the input string, the entire trimmed string is returned.
    /// This method does not throw an exception if the delimiter is missing.</remarks>
    /// <param name="value">The input string to parse. Can be empty or contain whitespace.</param>
    /// <param name="delimiter">The character that delimits the substring to extract.</param>
    /// <returns>A trimmed substring from the start of the input string up to (but not including) the first occurrence of the
    /// delimiter. Returns an empty string if the input is empty or consists only of whitespace.</returns>
    public static string Parse(this string value, char delimiter)
    {
        if (value.Trim().Length == 0) return "";
        string result = value.Substring(0, value.IndexOf(delimiter)).Trim();
        return result;
    }
    /// <summary>
    /// Parses the specified string using the default delimiter.
    /// </summary>
    /// <param name="value">The string to parse. Cannot be null.</param>
    /// <returns>A string representing the parsed result based on the default delimiter.</returns>
    public static string Parse(this string value)
    {
        // Parse on default delimiter
        return value.Parse(';');
    }
    /// <summary>
    /// Determines whether all characters in the specified string are contained within a set of allowed characters.
    /// </summary>
    /// <remarks>This method performs a case-sensitive comparison. If <paramref name="value"/> is empty, the
    /// method returns true.</remarks>
    /// <param name="value">The string to validate. Each character in this string is checked against the set of allowed characters.</param>
    /// <param name="check">A string containing the set of allowed characters. Each character in <paramref name="value"/> must exist in this
    /// string.</param>
    /// <returns>true if every character in <paramref name="value"/> is found in <paramref name="check"/>; otherwise, false.</returns>
    public static bool CheckString(this string value, string check)
    {
        foreach (char c in value)
        {
            if (check.IndexOf(c) == -1) return false;
        }
        return true;
    }
    /// <summary>
    /// Разделение строки на подстроки с разделителем delimiter 
    /// </summary>
    /// <param name="str">Строка с разделителями </param>
    /// <param name="delimiter">Символ разделитель</param>
    /// <returns>Коллекция строк</returns>
    public static StringCollection ToStringCollection(this string str, char delimiter = ';')
    {
        StringCollection result = new StringCollection();
        int i = 0;
        while (i < str.Length)
        {
            int pos = str.IndexOf(delimiter, i);
            if (pos == -1)                                          // End
            {
                // last substring
                result.Add(str.Substring(i).Trim());
                return result;
            }
            result.Add(str.Substring(i, pos - i).Trim());
            i = pos + 1;
        }
        return result;
    }
    /// <summary>
    /// Converts each character in the specified string to its corresponding uppercase Russian character, if applicable.
    /// </summary>
    /// <remarks>This method is an extension method for the <see cref="string"/> type. If the input string
    /// contains characters that do not have a Russian uppercase equivalent, those characters are left
    /// unchanged.</remarks>
    /// <param name="value">The input string to convert. Cannot be null.</param>
    /// <returns>A new string in which each character has been converted to its uppercase Russian equivalent. Characters without
    /// a Russian mapping remain unchanged.</returns>
    public static string ToRussian(this string value)
    {
        StringBuilder s = new StringBuilder(value);
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = s[i].UpperKeyToRus();
        }
        return s.ToString();
    }
    /// <summary>
    /// Converts the characters in the specified string to their Ukrainian equivalents using a predefined mapping.
    /// </summary>
    /// <remarks>Characters that do not have a Ukrainian equivalent in the mapping remain unchanged in the
    /// returned string.</remarks>
    /// <param name="value">The input string to convert. Cannot be null.</param>
    /// <returns>A new string in which each character from the input has been replaced with its Ukrainian equivalent. If the
    /// input string is empty, returns an empty string.</returns>
    public static string ToUkrainian(this string value)
    {
        StringBuilder s = new StringBuilder(value);
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = s[i].UpperKeyToUkraine();
        }
        return s.ToString();
    }

    /// <summary>
    /// Converts each character in the specified string to its Unicode code point and returns a comma-separated string
    /// of these numeric values.
    /// </summary>
    /// <param name="value">The input string whose characters are to be converted. Can be null.</param>
    /// <returns>A comma-separated string of numeric Unicode code points representing each character in the input string. Returns
    /// an empty string if the input is null.</returns>
    public static string ToHeximal(this string value)
    {
        StringBuilder result = new StringBuilder();
        if (value != null)
        {
            foreach (char c in value)
            {
                result.Append(((UInt16)c).ToString());
                result.Append(',');
            }
        }
        return result.ToString();

    }
    /// <summary>
    /// Converts a comma-separated string of 16-bit unsigned integer values to their corresponding Unicode characters
    /// and returns the resulting string.
    /// </summary>
    /// <param name="value">A string containing comma-separated 16-bit unsigned integer values representing Unicode code points.</param>
    /// <returns>A string composed of the Unicode characters corresponding to the parsed integer values. Returns an empty string
    /// if the input is null.</returns>
    /// <exception cref="Exception">Thrown if any of the comma-separated values cannot be parsed as a valid 16-bit unsigned integer.</exception>
    public static string FromHeximal(this string value)
    {
        StringBuilder result = new StringBuilder();
        var st = value?.ToStringCollection(',');
        if (st != null)
        {
            foreach (var s in st)
            {
                if (UInt16.TryParse(s, out ushort rs))
                    result.Append((char)rs);
                else
                    throw new Exception("System Error !!!");
            }
        }
        return result.ToString();

    }

    /// <summary>
    /// Use . or , for decimal separator
    /// </summary>
    /// <param name="value">string with decimal simbols</param>
    /// <param name="result">decimal value</param>
    /// <returns>true is succes</returns>
    public static bool TryToDecimal(this string value, out decimal result)
    {
        if (Decimal.TryParse(value, out result)) return true;
        if (Decimal.TryParse(value.Replace('.', ','), out result)) return true;
        return false;

    }
    static readonly string[] ukrToLatinChars = new string[]
    {
        "A",//А
        "B",//Б
        "V",//В
        "H",//Г
        "D",//Д
        "E",//Е
        "Zh",//Ж
        "Z",//З
        "Y",//И
        "I",//Й - last position
        "K",//K
        "L",//Л
        "M",//M
        "N",//H
        "O",//O
        "P",//П
        "R",//Р
        "S",//С
        "T",//T
        "U",//У
        "F",//Ф
        "Kh",//Х
        "Ts",//Ц
        "Ch",//Ч
        "Sh",//Ш
        "Shch",//Щ
        "",//Ъ
        "Y",//Ы
        "",//Ь
        "Е",//Э
        "Iu",//Ю Yu
        "Ia",//Я Ya
        "a",//А
        "b",//Б
        "v",//В
        "h",//Г
        "d",//Д
        "e",//Е
        "zh",//Ж
        "z",//З
        "y",//И
        "i",//Й - last position
        "k",//K
        "l",//Л
        "m",//M
        "n",//H
        "o",//O
        "p",//П
        "r",//Р
        "s",//С
        "t",//T
        "u",//У
        "f",//Ф
        "kh",//Х
        "ts",//Ц
        "ch",//Ч
        "sh",//Ш
        "shch",//Щ
        "",//Ъ
        "y",//Ы
        "",//Ь
        "e",//Э
        "iu",//Ю Yu
        "ia",//Я Ya
        };
    static readonly string[] ukrToLatinCharsFirst = new string[]
    {
        "A",//А
        "B",//Б
        "V",//В
        "H",//Г
        "D",//Д
        "E",//Е
        "Zh",//Ж
        "Z",//З
        "Y",//И
        "Y",//Й 
        "K",//K
        "L",//Л
        "M",//M
        "N",//H
        "O",//O
        "P",//П
        "R",//Р
        "S",//С
        "T",//T
        "U",//У
        "F",//Ф
        "Kh",//Х
        "Ts",//Ц
        "Ch",//Ч
        "Sh",//Ш
        "Shch",//Щ
        "",//Ъ
        "Y",//Ы
        "",//Ь
        "Е",//Э
        "Yu",//Ю Yu
        "Ya",//Я Ya
        "a",//А
        "b",//Б
        "v",//В
        "h",//Г
        "d",//Д
        "e",//Е
        "zh",//Ж
        "z",//З
        "y",//И
        "y",//Й
        "k",//K
        "l",//Л
        "m",//M
        "n",//H
        "o",//O
        "p",//П
        "r",//Р
        "s",//С
        "t",//T
        "u",//У
        "f",//Ф
        "kh",//Х
        "ts",//Ц
        "ch",//Ч
        "sh",//Ш
        "shch",//Щ
        "",//Ъ
        "y",//Ы
        "",//Ь
        "e",//Э
        "yu",//Ю Yu
        "ya",//Я Ya
    };

    /// <summary>
    /// Transliterates a Ukrainian string to its Latin-script representation according to official Ukrainian
    /// transliteration rules.
    /// </summary>
    /// <remarks>This method is intended for converting Ukrainian Cyrillic text to Latin characters, following
    /// the official Ukrainian transliteration standard. Non-Ukrainian characters are preserved as-is in the
    /// output.</remarks>
    /// <param name="value">The input string containing Ukrainian text to be transliterated.</param>
    /// <returns>A string containing the Latin-script transliteration of the input. If the input is empty, returns an empty
    /// string.</returns>
    public static string TranslitFromUkraineToLatin(this string value)
    {
        
        var result = new StringBuilder();
        int i = 0;
        bool isFirst = true;
        string getFromTable(char c, int lastIndex)
            {
                int index = (int)c - (int)'А';
                string s = isFirst ? ukrToLatinCharsFirst[index] : ukrToLatinChars[index];
                if (Char.IsLower(c)) return s;
                if (s.Length == 1) return s;
                if (lastIndex >= value.Length) return s.ToUpper(); // upper last simbol  
                if (char.IsUpper(value[lastIndex])) return s.ToUpper(); // last simbol is upper
                return s;
            }

        while (i < value.Length)
        {
            char c = value[i];
            i++;
            if (c == 'З')
            {
                    if (i<value.Length){
                        if (value[i] == 'Г')
                        {
                            result.Append("ZGh");
                            i++;
                            isFirst = false;
                            continue;
                        }
                        if (value[i] == 'г')
                        {
                            result.Append("Zgh");
                            i++;
                            isFirst = false;
                            continue;
                        }

                    }
            }   
            if (c == 'з')
            {
                    if (i<value.Length){
                        if (value[i] == 'Г')
                        {
                            result.Append("zGh");
                            i++;
                            isFirst = false;
                            continue;
                        }
                        if (value[i] == 'г')
                        {
                            result.Append("zgh");
                            i++;
                            isFirst = false;
                            continue;
                        }
                    }
            }   



            switch (c)
            {
                case '’':
                    continue;
                case ' ':
                    result.Append(c);
                    isFirst = true;
                    continue;
                case 'Ґ':
                    result.Append('G');
                    break;
                case 'ґ':
                    result.Append('g');
                    break;
                case 'Є':
                    result.Append(isFirst ? "Ye" : "Ie");
                    break;
                case 'є':
                    result.Append(isFirst ? "ye" : "ie");
                    break;
                case 'Ї':
                    result.Append(isFirst ? "Yi" : "I");
                    break;
                case 'ї':
                    result.Append(isFirst ? "yi" : "i");
                    break;
                case 'І':
                    result.Append('I');
                    break;
                case 'і':
                    result.Append('i');
                    break;
                   
                default:
                    if (c >= 'А' && c <= 'я')
                        result.Append(getFromTable(c, i));
                    else 
                       result.Append(c);
                    break;
            }
            isFirst = false;
        }
        return result.ToString();
    }
        
}
