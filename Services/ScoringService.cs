using TetrisConsole.Constants;

namespace TetrisConsole.Services;

/// <summary>
/// Service that handles the scoring logic of a Tetris game.
/// </summary>
public class ScoringService
{
    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int Score => _score;

    /// <summary>
    /// Gets the total number of lines cleared.
    /// </summary>
    public int LinesCleared => _totalLinesCleared;

    /// <summary>
    /// Adds a score based on the number of lines cleared in one move.
    /// </summary>
    /// <param name="clearedLines">The number of lines cleared in a single move.</param>
    public void AddScoreForClearedLines(int linesScored)
    {
        int multiplier = Math.Min(Math.Max(_totalLinesCleared / 10, GameConstants.MinLevel), GameConstants.MaxLevel);

        switch (linesScored)
        {
            case 1:
                _score += multiplier * 100;
                _totalLinesCleared++;
                break;
            case 2:
                _score += multiplier * 300;
                _totalLinesCleared += linesScored;
                break;
            case 3:
                _score += multiplier * 500;
                _totalLinesCleared += linesScored;
                break;
            case 4:
                _score += multiplier * 800;
                _totalLinesCleared += linesScored;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Represents current score of the game.
    /// </summary>
    private int _score;

    /// <summary>
    /// Keeps track of the total number of lines cleared by the player during the game. 
    /// </summary>
    private int _totalLinesCleared;
}
