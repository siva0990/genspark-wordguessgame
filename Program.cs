using System;

class Program
{
    static void Main()
    {
        try
        {
            IWordProvider wordProvider = new WordProvider();
            IGuessValidator validator = new GuessValidator();
            IFeedbackGenerator feedbackGenerator = new FeedbackGenerator();

            Game game = new Game(wordProvider, validator, feedbackGenerator);
            game.Start();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[CRITICAL ERROR] " + ex.Message);
            Console.ResetColor();
        }
        finally
        {
            Console.WriteLine("\nApplication closing. Press any key...");
            Console.ReadKey();
        }
    }
}