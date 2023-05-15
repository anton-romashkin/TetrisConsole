using System.Drawing;
using TetrisConsole.Constants;
using TetrisConsole.Enums;
using TetrisConsole.Models;

namespace TetrisConsole.Extensions;

/// <summary>
/// Provides extension methods for the FigureModel class.
/// </summary>
public static class FigureModelExtension
{
    /// <summary>
    /// Initializes a new instance of the FigureModel class.
    /// </summary>
    /// <param name="figure">The figure to initialize.</param>
    /// <param name="type">The type of the figure.</param>
    public static void Initialize(this FigureModel figure, FigureTypeEnum type)
    {
        figure.Position = new Point(GameConstants.PlayZoneWidth / 2 - GameConstants.BlockSize, 0);
        figure.Angle = RotationStateTypeEnum.ZeroDegreesState;
        figure.Type = type;
        figure.Blocks = GetFigureBlocks(figure);
        figure.Color = GetColor(figure);
    }

    /// <summary>
    /// Gets the blocks that make up the figure.
    /// </summary>
    /// <param name="figure">The figure to get the blocks for.</param>
    /// <returns>An array of Point objects that represent the blocks.</returns>
    public static Point[] GetFigureBlocks(FigureModel figure)
    {
        return figure.Type switch
        {
            FigureTypeEnum.IType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3) },
                _ => Array.Empty<Point>()
            },

            FigureTypeEnum.JType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(1, 0), new Point(2, 0), new Point(1, 1), new Point(1, 2) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(0, 2), new Point(1, 0), new Point(1, 1), new Point(1, 2) },
                _ => Array.Empty<Point>()
            },

            FigureTypeEnum.LType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 2) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(0, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(1, 2) },
                _ => Array.Empty<Point>()
            },

            FigureTypeEnum.OType => new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) },

            FigureTypeEnum.SType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(1, 0), new Point(1, 1), new Point(2, 1), new Point(2, 2) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(1, 1), new Point(2, 1), new Point(0, 2), new Point(1, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2) },
                _ => Array.Empty<Point>()
            },

            FigureTypeEnum.TType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(1, 0), new Point(1, 1), new Point(2, 1), new Point(1, 2) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2) },
                _ => Array.Empty<Point>()
            },

            FigureTypeEnum.ZType => figure.Angle switch
            {
                RotationStateTypeEnum.ZeroDegreesState => new[] { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1) },
                RotationStateTypeEnum.NinetyDegreesState => new[] { new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(0, 2) },
                RotationStateTypeEnum.OneEightyDegreesState => new[] { new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 2) },
                RotationStateTypeEnum.TwoSeventyDegreesState => new[] { new Point(2, 0), new Point(2, 1), new Point(1, 1), new Point(1, 2) },
                _ => Array.Empty<Point>()
            },

            _ => Array.Empty<Point>()
        };
    }

    /// <summary>
    /// Creates a new copy of the specified figure.
    /// </summary>
    /// <param name="figure">Figure to copy.</param>
    /// <returns>Copy of the specified figure.</returns>
    public static FigureModel Copy(this FigureModel figure)
    {
        var blocksCopy = figure.Blocks.Select(p => new Point(p.X, p.Y)).ToArray();

        return new FigureModel
        {
            Blocks = blocksCopy,
            Color = figure.Color,
            Position = new Point(figure.Position.X, figure.Position.Y),
            Type = figure.Type,
            Angle = figure.Angle
        };
    }

    /// <summary>
    /// Maps a type of a figure to a color and returns this color.
    /// </summary>
    /// <param name="figure">The figure to get the color for.</param>
    /// <returns>ConsoleColor based on the type of the figure.</returns>
    private static ConsoleColor GetColor(FigureModel figure) =>
        figure.Type switch
        {
            FigureTypeEnum.IType => ConsoleColor.Cyan,
            FigureTypeEnum.JType => ConsoleColor.Blue,
            FigureTypeEnum.LType => ConsoleColor.DarkYellow,
            FigureTypeEnum.OType => ConsoleColor.Yellow,
            FigureTypeEnum.SType => ConsoleColor.Green,
            FigureTypeEnum.TType => ConsoleColor.Magenta,
            FigureTypeEnum.ZType => ConsoleColor.Red,
            _ => ConsoleColor.Red
        };
}
