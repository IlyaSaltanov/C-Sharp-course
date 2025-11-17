



class Program
{
    private static string ApplyCommands(string[] commands)
    {
        string str = "";
        for (int i = 0; i < commands.Length; i++)
        {
            string line = commands[i];
            string[] elements = line.Split();
            if (elements[0] == "push")
            {
                    str += line.Substring(5);
            }
            else
            {
                str = str.Substring(0, str.Length - int.Parse(elements[1]));
            }

        }
        return str;
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