using System.ComponentModel;
using System.Runtime.ExceptionServices;
using MyNamespace;



class Program
{
    static void AddNumber(MyClass task)
    {
        Console.WriteLine("Выберите метод: MyPush, MyPop, GetMax");
        var inputMethod = Console.ReadLine();
        Console.WriteLine("Введите число");
        var variable = int.Parse(Console.ReadLine());

        if (inputMethod == "MyPush")
        {
            task.MyPush(variable);
        }
    }
    static void Main(string[] args)
    {
        var task = new MyClass();

        task.MyPush(5);
        task.MyPush(3);
        task.MyPush(7);


        Console.WriteLine(task.first.Peek());
        Console.WriteLine(task.GetMax());

        Console.WriteLine(task.MyPop());
        Console.WriteLine(task.MyPop());

        Console.WriteLine(task.first.Peek());






        while (inputMethod != "quit")
        {


            foreach (int i in task.first)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("Выберите метод: MyPush, MyPop, GetMax");
            inputMethod = Console.ReadLine();
            Console.WriteLine("Введите число");
            variable = int.Parse(Console.ReadLine());
        }
    }
}

// Дополнительные элементы можно добавить здесь