using System.Text.RegularExpressions;
using System.Linq;

public class GuessValidator : IGuessValidator
{
    public void Validate(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new InvalidGuessException("Input cannot be empty! Please type a 5-letter word and press Enter.");

        if (input.Length < 5)
            throw new InvalidGuessException("Guess is too short! Please enter exactly 5 characters to match the secret word.");

        if (input.Length > 5)
            throw new InvalidGuessException("Guess is too long! Please enter exactly 5 characters to match the secret word.");

        if (input.Any(char.IsDigit))
            throw new InvalidGuessException("Numbers are not allowed! Please use only alphabetic letters from A to Z.");

        if (Regex.IsMatch(input, @"[^a-zA-Z]"))
            throw new InvalidGuessException("Special characters are not allowed! Please use only alphabetic letters.");
            
        if (!Regex.IsMatch(input, "^[a-zA-Z]+$"))
            throw new InvalidGuessException("Invalid format! Please ensure you are entering only letters without spaces or symbols.");
    }
}