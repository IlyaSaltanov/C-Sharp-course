


// class Program
// {
//     static void Main(string[] args)
//     {
//         // Здесь будет ваш код
//         int n = int.Parse(Console.ReadLine());
//         int[] sp = new int[n];
//         int count = 1;
//         for (int i = 0; i < n; i++)
//         {
//             sp[i] = count;
//             count++;
//         }
//         var result = string.Join(", ", sp);
//         Console.WriteLine(result);

//         int k = int.Parse(Console.ReadLine());
//         Console.WriteLine(sp[sp.Length - k]);
//     }
// }

// // Дополнительные элементы можно добавить здесь







class Program
{
    public static int[] Sort(int[] arr)
    {
        int col = arr.Length * 2;
        int k = 0;
        while (col != 0)
        {
            bool flag = true;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    var nextNumbre = arr[i + 1];
                    var currentNumber = arr[i];
                    arr[i + 1] = currentNumber;
                    arr[i] = nextNumbre;
                    k++;
                    flag = false;
                }
            }
            if (flag)
            {
                break;
            }
            col -= 1;
        }
        return arr;
    }
    static void Main(string[] args)
    {
        // var sp = Sort([2, 5, 7, 2, 241, 4, 100]);
        var sp = Sort(new int[] { 2, 5, 7, 2, 241, 4, 100 });
        // { 2, 5, 7, 2, 241, 4, 100 }

        // { 2, 5, 2, 7, 241, 4, 100}
        // { 2, 5, 2, 7, 4, 241, 100}
        // { 2, 5, 2, 7, 4, 100, 241}
        // { 2, 2, 5, 7, 4, 100, 241}
        // { 2, 2, 5, 4, 7, 100, 241}
        // { 2, 2, 4, 5, 7, 100, 241}
        var result = string.Join(", ", sp);
        Console.WriteLine(result);
    }
}

// Дополнительные элементы можно добавить здесь