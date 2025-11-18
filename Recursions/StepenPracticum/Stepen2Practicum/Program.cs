



class Program
{
    static int Stepen(int number)
    {
        return 0;
    }
    static int Alg(int number, int step)
    {
        int k = 1;
        
        for (int i = step; i >= 0; i--)
        {
            k = number * number;
        }
        return k;
    }
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        Console.WriteLine(Alg(5, 2));
    }
}

// Дополнительные элементы можно добавить здесь