using System;
using System.Collections.Generic;

public class Program
{
    public static string FindLCS(string s1, string s2)
    {
        // Бинарный поиск по длине
        int left = 0;
        int right = Math.Min(s1.Length, s2.Length);
        string result = "";

        while (left <= right)
        {
            int mid = (left + right) / 2;
            string found = HasCommonSubstringOfLength(s1, s2, mid);

            if (found != null)
            {
                result = found;
                left = mid + 1; // Пробуем найти длиннее
            }
            else
            {
                right = mid - 1; // Уменьшаем длину
            }
        }

        return result;
    }

    private static string HasCommonSubstringOfLength(string s1, string s2, int length)
    {
        if (length == 0) return "";

        // Храним все подстроки длины length из первой строки
        HashSet<string> substrings = new HashSet<string>();
        for (int i = 0; i <= s1.Length - length; i++)
        {
            substrings.Add(s1.Substring(i, length));
        }

        // Ищем такую же во второй строке
        for (int i = 0; i <= s2.Length - length; i++)
        {
            string sub = s2.Substring(i, length);
            if (substrings.Contains(sub))
            {
                return sub;
            }
        }

        return null;
    }

    public static void Main()
    {
        Console.WriteLine(FindLCS("bab", "caba")); // "ab"
        Console.WriteLine(FindLCS("www.hankcs.com", "hankcs")); // "hankcs"
        Console.WriteLine(FindLCS("abcdef", "zcdem")); // "cde"
    }
}
