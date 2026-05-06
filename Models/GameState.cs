using System.Collections.Generic;

public class GameState
{
    public string SecretWord { get; set; }
    public int Attempts { get; set; }
    public int Score { get; set; }
    public string Difficulty { get; set; }
    public int HighScore { get; set; }

    public IGenericRepository<string> PreviousGuesses { get; set; }
    
    public Dictionary<char, char> LetterStatus { get; set; }

    public GameState()
    {
        PreviousGuesses = new GenericRepository<string>();
        Attempts = 0;
        Score = 0;
        Difficulty = "Easy";
        HighScore = 0;
        
        LetterStatus = new Dictionary<char, char>();
        for (char c = 'A'; c <= 'Z'; c++)
        {
            LetterStatus[c] = ' ';
        }
    }

    public string this[int index]
    {
        get => PreviousGuesses[index];
    }

    public void UpdateLetterStatus(string guess, string feedback)
    {
        for (int i = 0; i < 5; i++)
        {
            char letter = guess[i];
            char status = feedback[i];

            if (!LetterStatus.ContainsKey(letter)) continue;

            if (LetterStatus[letter] != 'G')
            {
                if (status == 'G' || (status == 'Y' && LetterStatus[letter] != 'Y') || (status == 'X' && LetterStatus[letter] == ' '))
                {
                    LetterStatus[letter] = status;
                }
            }
        }
    }
}