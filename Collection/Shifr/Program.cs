



class Program
{
    static string DecodeMessage(string[] lines)
    {
        List<string> myList = new List<string>();
        for (int i = 0;i < lines.Length; i++)
        {
            string stroka = lines[i];
            string[] spStroka = stroka.Split();
            for (int q = 0; q < spStroka.Length ; q++)
            {
                string strElemnt = spStroka[q];
                // if (strElemnt[0] == char.ToUpper(strElemnt[0]))
                // {
                //     myList.Add(strElemnt);
                // }   
                if (!string.IsNullOrEmpty(strElemnt) && char.IsUpper(strElemnt[0]))
                {
                    myList.Add(strElemnt);
                }   
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

        // string[] lines = {"Сдавайся", "НЕ", "твоего", "ума", "Ты", "не", "споСОбЕн", "Но", "может", "быть"};
        // string[] lines = {"Сдавайся НЕ твоего ума Ты не споСОбЕн Но может быть", "очень ХОРОШИЙ код", "и я буДу Писать тЕбЕ еще"};
        string[] lines =
        {
            "решИла нЕ Упрощать и зашифРОВАтЬ Все послаНИЕ",
            "дАже не Старайся нИЧЕГО у тЕбя нЕ получится с расшифРОВкой",
            "Сдавайся НЕ твоего ума Ты не споСОбЕн Но может быть",
            "если особенно упорно подойдешь к делу",

            "будет Трудно конечнО",
            "Код ведЬ не из простых",
            "очень ХОРОШИЙ код",
            "то у тебя все получится",
            "и я буДу Писать тЕбЕ еще",

            "чао"
        };
        Console.WriteLine(DecodeMessage(lines));
    }
}

// Дополнительные элементы можно добавить здесь