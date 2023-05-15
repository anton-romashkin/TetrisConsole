using System.Drawing;
using TetrisConsole.Enums;

namespace TetrisConsole.Models;

/// <summary>
/// Represents a tetromino in the game of Tetris.
/// </summary>
public class FigureModel
{
    /// <summary>
    /// Position of each block in the tetromino.
    /// </summary>
    public Point[] Blocks { get; set; } = Array.Empty<Point>();

    /// <summary>
    /// Color of the tetromino.
    /// </summary>
    public ConsoleColor Color { get; set; }

    /// <summary>
    /// Position of the tetromino on the game board.
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    /// Type of tetromino.
    /// </summary>
    public FigureTypeEnum Type { get; set; }

    /// <summary>
    /// Current angle of the tetromino.
    /// </summary>
    public RotationStateTypeEnum Angle { get; set; }
}