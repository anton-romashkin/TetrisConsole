using System;
using System.Drawing;
using TetrisConsole.Constants;
using TetrisConsole.Models;

namespace TetrisConsole.Services;

/// <summary>
/// Service for rendering various elements of a Tetris game on the console.
/// </summary>
public class DisplayService
{
    /// <summary>
    /// Draws the border of the play zone.
    /// </summary>
    public void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        int offsetX = _boardOffsetX;
        int borderStartRow = _borderStartRow;

        for (int y = _borderStartRow; y < GameConstants.PlayZoneHeight; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("██");

            Console.SetCursorPosition(GameConstants.PlayZoneWidth * GameConstants.BlockSize + offsetX, y);
            Console.Write("██");

        }

        for (int x = 0; x < GameConstants.PlayZoneWidth + GameConstants.BlockSize; x++)
        {
            Console.SetCursorPosition(x * GameConstants.BlockSize, GameConstants.PlayZoneHeight);
            Console.Write("██");
        }

        Console.ResetColor();
        Console.CursorVisible = false;
    }

    /// <summary>
    /// Draws the current state of the game board.
    /// </summary>
    /// <param name="board">Game board.</param>
    public void DrawGameBoard(int[,] board)
    {
        int offsetX = _boardOffsetX;

        for (int x = 0; x < GameConstants.PlayZoneWidth; x++)
        {
            for (int y = 0; y < GameConstants.PlayZoneHeight; y++)
            {
                Console.SetCursorPosition(x * GameConstants.BlockSize + offsetX, y);

                if (board[x, y] == 0)
                {
                    Console.Write("  ");
                    continue;
                }

                Console.ForegroundColor = (ConsoleColor)board[x, y];
                Console.Write("██");
            }
        }

        Console.SetCursorPosition(25, 25);
    }

    /// <summary>
    /// Erases the figure from the game board.
    /// </summary>
    /// <param name="currentFigure">Figure to erase.</param>
    public void EraseFigure(FigureModel? currentFigure)
    {
        if (currentFigure == null)
        {
            return;
        }

        int offsetX = _boardOffsetX;

        foreach (Point block in currentFigure.Blocks)
        {
            int x = (currentFigure.Position.X + block.X) * GameConstants.BlockSize + offsetX;
            int y = currentFigure.Position.Y + block.Y;

            Console.SetCursorPosition(x, y);
            Console.Write("  ");
        }

        //Console.ResetColor();
        Console.SetCursorPosition(25, 25);
    }

    /// <summary>
    /// Draws the figure on the game board.
    /// </summary>
    /// <param name="currentFigure">Figure to draw.</param>
    public void DrawFigure(FigureModel? currentFigure)
    {
        if (currentFigure == null)
        {
            return;
        }

        Console.ForegroundColor = currentFigure.Color;

        int offsetX = _boardOffsetX;

        foreach (Point block in currentFigure.Blocks)
        {
            int x = (currentFigure.Position.X + block.X) * GameConstants.BlockSize + offsetX;
            int y = currentFigure.Position.Y + block.Y;

            Console.SetCursorPosition(x, y);
            Console.Write("██");
        }

        Console.ResetColor();
        Console.SetCursorPosition(25, 25);
    }

    /// <summary>
    /// Displays the next and hold figures, score and diffility level in the console.
    /// </summary>
    /// <param name="nextFigureSection">Next figure as int[,].</param>
    /// <param name="holdFigureSection">Held figure as int[,].</param>
    /// <param name="score">Current score.</param>
    /// <param name="level">Current level.</param>
    public void DisplayInterface(int[,] nextFigureSection, int[,] holdFigureSection, int score, int level)
    {
        Console.ResetColor();

        int offsetX = _interfaceOffsetX;
        int offsetY = 3;

        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write("NEXT:");
        offsetY ++;

        for (int x = 0; x < GameConstants.FigureSectionWidth; x++)
        {
            for (int y = 0; y < GameConstants.FigureSectionHeight; y++)
            {
                Console.SetCursorPosition(x * GameConstants.BlockSize + offsetX, y + offsetY);

                if (nextFigureSection[x, y] == 0)
                {
                    Console.Write("  ");
                    continue;
                }

                Console.ForegroundColor = (ConsoleColor)nextFigureSection[x, y];
                Console.Write("██");
            }
        }

        offsetY += 3;

        Console.ResetColor();

        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write("HOLD:");
        offsetY ++;

        for (int x = 0; x < GameConstants.FigureSectionWidth; x++)
        {
            for (int y = 0; y < GameConstants.FigureSectionHeight; y++)
            {
                Console.SetCursorPosition(x * GameConstants.BlockSize + offsetX, y + offsetY);

                if (holdFigureSection[x, y] == 0)
                {
                    Console.Write("  ");
                    continue;
                }

                Console.ForegroundColor = (ConsoleColor)holdFigureSection[x, y];
                Console.Write("██");
            }
        }

        offsetY += 3;

        Console.ResetColor();

        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write("SCORE:");
        offsetY++;
        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write(score);
        offsetY += 2;

        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write("LEVEL:");
        offsetY++;
        Console.SetCursorPosition(offsetX, offsetY);
        Console.Write(level);
        offsetY += 2;

        Console.SetCursorPosition(25, 25);
    }

    /// <summary>
    /// Displays the end game screen with the final score.
    /// </summary>
    /// <param name="score">Score.</param>
    public void DisplayEndGameScreen(int score)
    {
        Console.Clear();
        Console.SetCursorPosition(GameConstants.PlayZoneWidth, GameConstants.PlayZoneHeight / 2);
        Console.Write("GAME OVER");
        Console.SetCursorPosition(GameConstants.PlayZoneWidth, GameConstants.PlayZoneHeight / 2 + 1);
        Console.Write($"SCORE:{score}");
        Console.SetCursorPosition(25, 25);
    }

    private int _boardOffsetX = GameConstants.BlockSize;

    private int _borderStartRow = 3;

    private int _interfaceOffsetX = 30;
}

