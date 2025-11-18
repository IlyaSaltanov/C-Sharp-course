

class Program
{
    static int Sum(int[] list, int k, int s = 0)
    {
        if (k >= list.Length) return s;
        return Sum(list, k+1, s + list[k]);
        
    }
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        Console.WriteLine(Sum(new int[] { 1, 2, 4, 5 }, 0));
    }
}

// Дополнительные элементы можно добавить здесь