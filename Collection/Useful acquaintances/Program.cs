

using System.Collections.Generic;


class Program
{
    static Dictionary<string, List<string>> OptimizeContacts(List<string> contacts)
    {
        var dictionary = new Dictionary<string, List<string>>();
        List<string> sp = new List<string>();
        for (int i = 0; i < contacts.Count; i++)
        {
            if (contacts[i].Contains("Sa"))
            {
                sp.Add(contacts[i]);
            }
        }

        return dictionary;
    }
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        Console.WriteLine("Hello, World!");
    }
}

// Дополнительные элементы можно добавить здесь
