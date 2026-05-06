using System;
using System.Linq;

public class Game
{
    private readonly IWordProvider _wordProvider;
    private readonly IGuessValidator _validator;
    private readonly IFeedbackGenerator _feedback;

    public Game(IWordProvider wp, IGuessValidator gv, IFeedbackGenerator fg)
    {
        _wordProvider = wp ?? throw new ArgumentNullException(nameof(wp));
        _validator = gv ?? throw new ArgumentNullException(nameof(gv));
        _feedback = fg ?? throw new ArgumentNullException(nameof(fg));
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       WELCOME TO WORD GUESSER          ");
                Console.WriteLine("========================================");
                Console.WriteLine($"CURRENT HIGH SCORE: {ScoreManager.GetHighScore()}");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("1. Easy (6 Attempts)");
                Console.WriteLine("2. Medium (5 Attempts)");
                Console.WriteLine("3. Hard (4 Attempts)");
                Console.WriteLine("4. Very Hard (3 Attempts)");
                Console.WriteLine("5. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine();
                if (choice == "5") break;

                int maxAttempts;
                string difficulty;

                switch (choice)
                {
                    case "1": difficulty = "Easy"; maxAttempts = 6; break;
                    case "2": difficulty = "Medium"; maxAttempts = 5; break;
                    case "3": difficulty = "Hard"; maxAttempts = 4; break;
                    case "4": difficulty = "Very Hard"; maxAttempts = 3; break;
                    default:
                        Console.WriteLine("\nInvalid selection! Select 1-5.");
                        System.Threading.Thread.Sleep(1000);
                        continue;
                }

                _wordProvider.SetDifficulty(difficulty);
                RunGameSession(difficulty, maxAttempts);

                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.ReadKey();
            }
        }
    }

    private void RunGameSession(string difficulty, int maxAttempts)
    {
        GameState state = new GameState
        {
            SecretWord = _wordProvider.GetRandomWord(),
            Difficulty = difficulty,
            HighScore = ScoreManager.GetHighScore()
        };

        string[] attemptComments = { "Genius!", "Excellent!", "Great job!", "Good work!", "Nice try!", "That was close!" };
        bool won = false;

        while (state.Attempts < maxAttempts)
        {
            try
            {
                DisplayAlphabetTracker(state);
                Console.WriteLine($"\n[{difficulty}] Attempt {state.Attempts + 1}/{maxAttempts}");
                Console.Write("Enter your 5-letter guess: ");
                string guess = Console.ReadLine()?.ToUpper() ?? "";

                _validator.Validate(guess);

                if (state.PreviousGuesses.Contains(guess))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("--> Already tried this word!");
                    Console.ResetColor();
                    continue;
                }

                state.PreviousGuesses.Add(guess);
                string feedback = _feedback.Generate(guess, state.SecretWord);
                
                state.UpdateLetterStatus(guess, feedback);
                DisplayFeedback(guess, feedback);

                Console.WriteLine($"\n{attemptComments[state.Attempts]}");
                state.Attempts++;

                if (feedback == "GGGGG")
                {
                    won = true;
                    state.Score = CalculateScore(state.Attempts, difficulty, maxAttempts);
                    break;
                }
            }
            catch (InvalidGuessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{ex.Message}");
                Console.ResetColor();
            }
        }

        DisplayGameResult(won, state);
    }

    private void DisplayAlphabetTracker(GameState state)
    {
        Console.WriteLine("\nALPHABET STATUS:");
        int count = 0;
        foreach (var kvp in state.LetterStatus)
        {
            switch (kvp.Value)
            {
                case 'G': Console.ForegroundColor = ConsoleColor.Green; break;
                case 'Y': Console.ForegroundColor = ConsoleColor.Yellow; break;
                case 'X': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                default: Console.ForegroundColor = ConsoleColor.White; break;
            }

            Console.Write($"{kvp.Key} ");
            count++;
            if (count % 13 == 0) Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine();
    }

    private void DisplayFeedback(string guess, string feedback)
    {
        Console.WriteLine("\nFEEDBACK:");
        for (int i = 0; i < 5; i++)
        {
            switch (feedback[i])
            {
                case 'G': Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                case 'Y': Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                default: Console.BackgroundColor = ConsoleColor.DarkGray; break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {guess[i]} ");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();
    }

    private void DisplayGameResult(bool won, GameState state)
    {
        if (won)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n****************************************");
            Console.WriteLine($"🎉 Victory on {state.Difficulty}!");
            Console.WriteLine($"Final Score: {state.Score}");
            
            if (ScoreManager.SaveHighScore(state.Score))
            {
                Console.WriteLine("🏆 NEW PERSONAL BEST RECORD!");
            }
            Console.WriteLine("****************************************");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine($"❌ Word was: {state.SecretWord}");
            Console.WriteLine("----------------------------------------");
        }
        Console.ResetColor();
    }

    private int CalculateScore(int attempts, string difficulty, int maxAttempts)
    {
        int multiplier = difficulty switch
        {
            "Easy" => 1,
            "Medium" => 2,
            "Hard" => 3,
            "Very Hard" => 4,
            _ => 1
        };
        return (maxAttempts - attempts + 1) * 20 * multiplier;
    }
}