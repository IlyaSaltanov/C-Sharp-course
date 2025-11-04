



class Program
{
    static string DecodeMessage(string[] lines)
    {
        List<string> myList = new List<string>();
        for (int i = 0; lines[i].Length > 0 && i < lines.Length; i++)
        {
            string strElemnt = lines[i];
            if (strElemnt[0] == char.ToUpper(strElemnt[0]))
            {
                myList.Add(strElemnt);
            }
        }
        string result = "";
        for ( int i = myList.Count-1 ; i >= 0 ; i = i - 1)
        {
            result = result + myList[i] + " ";
        }
        return result;
        
    }
    static void Main(string[] args)
    {
        string[] lines = {"Сдавайся", "НЕ", "твоего", "ума", "Ты", "не", "споСОбЕн", "Но", "может", "быть"};
        Console.WriteLine(DecodeMessage(lines));
    }
}

// Дополнительные элементы можно добавить здесь