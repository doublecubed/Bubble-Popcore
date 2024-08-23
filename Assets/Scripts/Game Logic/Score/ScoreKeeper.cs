// Onur Ereren - July 2024
// Popcore case


using System;

public class ScoreKeeper : IScoreKeeper
{
    private int _currentScore;

    public Action<int> OnScoreUpdated;
    
    public ScoreKeeper()
    {
        
    }
    
    
    public void AddScore(int value)
    {
        _currentScore += value;
        
        OnScoreUpdated?.Invoke(_currentScore);
    }

    public int CurrentScore()
    {
        return _currentScore;
    }
}
