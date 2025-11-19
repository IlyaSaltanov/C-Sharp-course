public class CaseAlternatorTask
{
    //Тесты будут вызывать этот метод
    public static List<string> AlternateCharCases(string lowercaseWord)
    {
        var result = new List<string>();
        AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
        return result;
	}

	static bool ShouldAlternateCase(char token)
    {
        return char.IsLetter(token) && token != 'ß' && char.ToLower(token) != char.ToUpper(token);
	}

    static void AlternateCharCases(char[] word, int startIndex, List<string> result)
    {
        if (startIndex == word.Length)
        {
            result.Add(new string(word));
            return;
        }

        char currentChar = word[startIndex];

		if (ShouldAlternateCase(currentChar))
        {
            char lowerChar = char.ToLower(currentChar);
            char upperChar = char.ToUpper(currentChar);

			word[startIndex] = lowerChar;
			AlternateCharCases(word, startIndex + 1, result);

			word[startIndex] = upperChar;
			AlternateCharCases(word, startIndex + 1, result);
        }
        else
        {
            AlternateCharCases(word, startIndex + 1, result);
        }
    }
}