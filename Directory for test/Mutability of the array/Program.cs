


class Program
{
    static void Main()
    {
        char[] chars = {'a', 'b', 'c'};
        char[] original = chars;  // обе переменные ссылаются на один массив

        chars[1] = 'B';

        Console.WriteLine(original[1]);  // Выведет 'B'!
        Console.WriteLine(chars[1]);     // Тоже выведет 'B'!
    }
}