using System;
using System.Collections.Generic;

public class WordProvider : IWordProvider
{
    private readonly List<string> _easyWords = new List<string> { "APPLE", "MANGO", "GRAPE", "PLANT", "BRAIN", "LEMON", "PEACH", "TRAIN" };
    private readonly List<string> _mediumWords = new List<string> { "CLOCK", "FLUTE", "STORM", "HOUSE", "BREAD", "PIANO", "TIGER", "WATER" };
    private readonly List<string> _hardWords = new List<string> { "QUART", "FJORD", "GYPSY", "VODKA", "WALTZ", "JAZZY", "NYMPH", "CRAZY" };
    private readonly List<string> _veryHardWords = new List<string> { "XYLYL", "PHLOX", "QUARTZ", "MYTHS", "SPHINX", "CRYPT", "DWARV", "LYNCH" };
    
    private List<string> _currentWordList;

    public WordProvider()
    {
        _currentWordList = _easyWords;
    }

    public void SetDifficulty(string difficulty)
    {
        if (string.IsNullOrEmpty(difficulty))
            throw new ArgumentException("Difficulty setting is required.");

        switch (difficulty.ToLower())
        {
            case "easy": _currentWordList = _easyWords; break;
            case "medium": _currentWordList = _mediumWords; break;
            case "hard": _currentWordList = _hardWords; break;
            case "very hard": _currentWordList = _veryHardWords; break;
            default: _currentWordList = _easyWords; break;
        }
    }

    public string GetRandomWord()
    {
        if (_currentWordList == null || _currentWordList.Count == 0)
            throw new InvalidOperationException("Available word list is empty.");

        Random random = new Random();
        return _currentWordList[random.Next(_currentWordList.Count)];
    }

    public bool IsValidWord(string word)
    {
        return true; 
    }

    public string this[int index]
    {
        get 
        {
            if (index < 0 || index >= _currentWordList.Count)
                throw new IndexOutOfRangeException("Requested word index is out of bounds.");

            return _currentWordList[index];
        }
    }
}