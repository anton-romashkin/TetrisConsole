using System;

namespace TetrisConsole.Constants;

/// <summary>
/// Constant game parameters.
/// </summary>
public static class GameConstants
{
    public const int PlayZoneWidth = 10;
    public const int PlayZoneHeight = 23;

    public const int FigureSectionWidth = 4;
    public const int FigureSectionHeight = 4;

    public const int BlockSize = 2;

    public const int FigureTypesCount = 7;

    public const int MinLevel = 1;
    public const int MaxLevel = 9;

    public const int InitialGameSpeed = 800;
}
