public interface IWordProvider
{
    string GetRandomWord();
    void SetDifficulty(string difficulty);
    bool IsValidWord(string word);
}