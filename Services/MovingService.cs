using System.Drawing;
using TetrisConsole.Helpers;
using TetrisConsole.Models;
using TetrisConsole.Enums;
using TetrisConsole.Constants;
using TetrisConsole.Extensions;

namespace TetrisConsole.Services;

/// <summary>
/// Responsible for handling the movement of the figures in the game.
/// </summary>
public class MovingService
{
    /// <summary>
    /// Checks if the specified position for the figure is valid on the game board.
    /// </summary>
    /// <param name="figure">Figure to check</param>
    /// <param name="position">Position on the board to check</param>
    /// <param name="board">Game board</param>
    /// <returns></returns>
    public bool IsMoveValid(FigureModel figure, Point position, int[,] board)
    {
        foreach (var block in figure.Blocks)
        {
            int x = position.X + block.X;
            int y = position.Y + block.Y;

            if (x < 0 || x >= GameConstants.PlayZoneWidth || y >= GameConstants.PlayZoneHeight || y < 0 || board[x, y] != 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Moves the current figure down one row if the move is valid, and returns true if the move was successful.
    /// </summary>
    /// <param name="currentFigure">Figure to move</param>
    /// <param name="board">Game board</param>
    /// <returns>Returns true if the move was successful</returns>
    public bool MoveDown(FigureModel currentFigure, int[,] board)
    {
        var newPosition = new Point(currentFigure.Position.X, currentFigure.Position.Y + 1);
        if (IsMoveValid(currentFigure, newPosition, board))
        {
            currentFigure.Position = newPosition;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Moves the current figure one column to the left if the move is valid.
    /// </summary>
    /// <param name="currentFigure">Figure to move</param>
    /// <param name="board">Game board</param>
    public void MoveLeft(FigureModel currentFigure, int[,] board)
    {
        var newPosition = new Point(currentFigure.Position.X - 1, currentFigure.Position.Y);
        if (IsMoveValid(currentFigure, newPosition, board))
        {
            currentFigure.Position = newPosition;
        }
    }

    /// <summary>
    /// Moves the current figure one column to the right if the move is valid.
    /// </summary>
    /// <param name="currentFigure">Figure to move</param>
    /// <param name="board">Game board</param>
    public void MoveRight(FigureModel currentFigure, int[,] board)
    {
        var newPosition = new Point(currentFigure.Position.X + 1, currentFigure.Position.Y);
        if (IsMoveValid(currentFigure, newPosition, board))
        {
            currentFigure.Position = newPosition;
        }
    }

    /// <summary>
    /// Drops the current figure to the bottom of the game board.
    /// </summary>
    /// <param name="currentFigure">Figure to move.</param>
    /// <param name="board">Game board.</param>
    public void DropFigure(FigureModel currentFigure, int[,] board)
    {
        while (MoveDown(currentFigure, board)) { }
    }

    /// <summary>
    /// Rotates the current figure clockwise and returns the rotated figure.
    /// </summary>
    /// <param name="currentFigure">Figure to rotate.</param>
    /// <param name="board">Game board.</param>
    /// <returns>If the move is valid, the rotated figure is returned, otherwise returns the original figure.</returns>
    public FigureModel Rotate(FigureModel currentFigure, int[,] board)
    {
        var rotatedFigure = currentFigure.Copy();

        rotatedFigure.Angle = MovingServiceHelper.GetNextRotationState(rotatedFigure);

        Point[] rotatedBlocks = FigureModelExtension.GetFigureBlocks(rotatedFigure);
        rotatedFigure.Blocks = rotatedBlocks;

        int positionX = rotatedFigure.Position.X;
        int positionY = rotatedFigure.Position.Y;

        Point[] positionOffsets = new[]
        {
            new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(-1,0), new Point(-2, 0),
            new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(-1, 1), new Point(-2, 1),
            new Point(0, -1), new Point(1, -1), new Point(2, -1), new Point(-1, -1), new Point(-2, -1)
        };

        foreach (var offset in positionOffsets)
        {
            var newPosition = new Point(positionX + offset.X, positionY + offset.Y);

            if (IsMoveValid(rotatedFigure, newPosition, board))
            {
                rotatedFigure.Position = newPosition;
                return rotatedFigure;
            }
        }
        return currentFigure;
    }
}
