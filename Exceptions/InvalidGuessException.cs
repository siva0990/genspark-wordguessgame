using System;

public class InvalidGuessException : Exception
{
    public InvalidGuessException() : base("An invalid guess was entered.")
    {
    }

    public InvalidGuessException(string message) : base(message)
    {
    }

    public InvalidGuessException(string message, Exception inner) : base(message, inner)
    {
    }
}
