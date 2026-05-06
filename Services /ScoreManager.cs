using System;
using System.IO;

public static class ScoreManager
{
    private const string FilePath = "highscore.txt";

    public static int GetHighScore()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string content = File.ReadAllText(FilePath);
                if (int.TryParse(content, out int score))
                {
                    return score;
                }
            }
        }
        catch
        {
        }
        return 0;
    }

    public static bool SaveHighScore(int newScore)
    {
        try
        {
            int currentHighScore = GetHighScore();
            if (newScore > currentHighScore)
            {
                File.WriteAllText(FilePath, newScore.ToString());
                return true;
            }
        }
        catch
        {
        }
        return false;
    }
}
