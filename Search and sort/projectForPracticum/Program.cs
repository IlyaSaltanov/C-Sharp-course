


class Program
{
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        int n = int.Parse(Console.ReadLine());
        int[] sp = new int[n];
        for (int i = 0; i < n; i++)
        {
            sp[i] = i;
        }
        var result = string.Join(", ", sp);
        Console.WriteLine(result);

        int k = int.Parse(Console.ReadLine());
        Console.WriteLine(sp[sp.Length - k]);
    }
}

// Дополнительные элементы можно добавить здесь