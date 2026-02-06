
using System;
using System.Diagnostics;
using System.Text;


class Program
{
    private static string ApplyCommands(string[] commands)
    {
        StringBuilder str = new StringBuilder("");
        for (int i = 0; i < commands.Length; i++)
        {
            string line = commands[i];
            string[] elements = line.Split();
            if (elements[0] == "push")
            {
                    str.Append(line.Substring(5));
            }
            else
            {
                str = str.Remove(str.Length - int.Parse(elements[1]), int.Parse(elements[1]));
            }

        }
        return str.ToString();
    }
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        string[] temp = {
            "push Привет! Это снова я! Пока!",
            "pop 5",
            "push Как твои успехи? Плохо?",
            "push qwertyuiop",
            "push 1234567890",
            "pop 26"
                        };
        Console.WriteLine(ApplyCommands(temp));
    }
}

// Дополнительные элементы можно добавить здесь