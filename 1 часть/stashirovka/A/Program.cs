



using System;

class Program
{
    static void Main()
    {
        string str = Console.ReadLine();
        int total_M = 0;
        foreach (char c in str)
        {
            if (c == 'M') total_M++;
        }
        
        int n = 1;
        while (n * (n + 1) / 2 <= total_M)
        {
            n++;
        }
        
        Console.WriteLine(n - 1);
    }
}



