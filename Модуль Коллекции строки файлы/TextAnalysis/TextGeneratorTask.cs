namespace TextAnalysis;

static class TextGeneratorTask
{
    public static string ContinuePhrase(
        Dictionary<string, string> nextWords,
        string phraseBeginning,
        int wordsCount)
    {
        return phraseBeginning;
    }
}// Вставьте сюда финальное содержимое файла TextGeneratorTask.cs

using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
            if (wordsCount == 0 || phraseBeginning == "") return phraseBeginning;
            var finalPhrase = new StringBuilder(phraseBeginning);
            var lastWords = phraseBeginning;
            var wordsInBuilder = 0;
            var result = AddWords(wordsInBuilder, lastWords, phraseBeginning, wordsCount, nextWords);
            return result;
        }

        public static string AddWords(int wordsInBuilder, string lastWords, string phraseBeginning, int wordsCount, Dictionary<string, string> nextWords)
        {
            var finalPhrase = new StringBuilder(phraseBeginning);
            string lastOneWord;
            
            for (var i = 0; i < wordsCount; i++)
            {
                wordsInBuilder = CountWordsInPhrase(finalPhrase.ToString());
                
                if (HasMultipleWords(wordsInBuilder))
                    lastWords = ExtractLastTwoWords(finalPhrase.ToString(), wordsInBuilder);

                lastOneWord = ExtractFinalWord(finalPhrase.ToString(), wordsInBuilder);

                if (!CanContinueGeneration(nextWords, lastWords, lastOneWord))
                    break;
                    
                string continuation = DetermineContinuation(nextWords, lastWords, lastOneWord);
                AppendWordToPhrase(finalPhrase, continuation);
            }
            
            return finalPhrase.ToString();
        }

        private static int CountWordsInPhrase(string phrase)
        {
            return phrase.Split(" ").Length;
        }

        private static bool HasMultipleWords(int wordCount)
        {
            return wordCount > 1;
        }

        private static string ExtractLastTwoWords(string phrase, int wordCount)
        {
            var words = phrase.Split(" ");
            return $"{words[wordCount - 2]} {words[wordCount - 1]}";
        }

        private static string ExtractFinalWord(string phrase, int wordCount)
        {
            var words = phrase.Split(" ");
            return words[wordCount - 1];
        }

        private static bool CanContinueGeneration(Dictionary<string, string> nextWords, 
                                                 string lastTwoWords, string lastWord)
        {
            return nextWords.ContainsKey(lastTwoWords) || nextWords.ContainsKey(lastWord);
        }

        private static string DetermineContinuation(Dictionary<string, string> nextWords, 
                                                   string lastTwoWords, string lastWord)
        {
            if (nextWords.ContainsKey(lastTwoWords))
                return nextWords[lastTwoWords];
            else
                return nextWords[lastWord];
        }

        private static void AppendWordToPhrase(StringBuilder phraseBuilder, string wordToAdd)
        {
            phraseBuilder.Append(" " + wordToAdd);
        }

        private static string[] SplitPhraseIntoWords(string phrase)
        {
            return phrase.Split(" ");
        }

        private static bool IsGenerationRequired(int wordsCount, string phraseBeginning)
        {
            return wordsCount > 0 && !string.IsNullOrEmpty(phraseBeginning);
        }

        private static void UpdateLastWords(StringBuilder phraseBuilder, out string lastTwoWords, out string lastWord)
        {
            var phrase = phraseBuilder.ToString();
            var wordCount = CountWordsInPhrase(phrase);
            
            if (HasMultipleWords(wordCount))
                lastTwoWords = ExtractLastTwoWords(phrase, wordCount);
            else
                lastTwoWords = phrase;
                
            lastWord = ExtractFinalWord(phrase, wordCount);
        }
    }
}
