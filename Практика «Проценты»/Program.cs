



using System.Data.SqlTypes;
using System.IO.Pipelines;

class Program
{
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        string st = Console.ReadLine();
        
        Console.WriteLine(Calculate(st));



        // string text = "Привет мир C#";
        // string[] words = text.Split(' ');
        // Console.WriteLine(words[0]);
        // foreach (string str in words)
        // {
        //     Console.WriteLine(str);
        // }
    }

    public static double Calculate(string userInput)
    {
        var list = userInput.Split(' ');
        // double money = double.Parse(list[0], System.Globalization.CultureInfo.InvariantCulture);
        double money = Convert.ToDouble(list[0], System.Globalization.CultureInfo.InvariantCulture);
        double percent = Convert.ToDouble(list[1]);
        double months = Convert.ToDouble(list[2]);
        double result = money * Math.Pow((1 + (percent / (100 * 12))), months);
        return result;
    }
}

// Дополнительные элементы можно добавить здесь