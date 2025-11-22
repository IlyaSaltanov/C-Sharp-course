using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        string[] pieces = new string[n];
        for (int i = 0; i < n; i++)
        {
            pieces[i] = Console.ReadLine();
        }
        
        Array.Sort(pieces, ComparePieces);
        
        Console.WriteLine(string.Join("", pieces));
    }
    
    private static int ComparePieces(string a, string b)
    {
        string ab = a + b;
        string ba = b + a;
        return ba.CompareTo(ab);
    }
}