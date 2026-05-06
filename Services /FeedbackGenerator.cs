using System;

public class FeedbackGenerator : IFeedbackGenerator
{
    public string Generate(string guess, string word)
    {
        if (string.IsNullOrEmpty(guess) || string.IsNullOrEmpty(word) || guess.Length != 5 || word.Length != 5)
            throw new ArgumentException("Guess and word must be exactly 5 characters long.");

        char[] result = { 'X', 'X', 'X', 'X', 'X' };
        bool[] wordUsed = new bool[5];

        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == word[i])
            {
                result[i] = 'G';
                wordUsed[i] = true;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (result[i] == 'G') continue;

            for (int j = 0; j < 5; j++)
            {
                if (!wordUsed[j] && guess[i] == word[j])
                {
                    result[i] = 'Y';
                    wordUsed[j] = true;
                    break;
                }
            }
        }

        return new string(result);
    }
}