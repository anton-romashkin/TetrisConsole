using System;

namespace TetrisConsole.Enums;

/// <summary>
/// Figure rotation states.
/// </summary>
public enum RotationStateTypeEnum
{
    /// <summary>
    /// The figure's angle is unknown
    /// </summary>
    UndefinedState,

    /// <summary>
    /// The figure is rotated by 0 degrees, meaning it is in its original orientation.
    /// </summary>
    ZeroDegreesState,

    /// <summary>
    /// The figure has been rotated clockwise by 90 degrees relative to its original orientation.
    /// </summary>
    NinetyDegreesState,

    /// <summary>
    /// The figure has been rotated by half a full rotation, or upside-down, relative to its original orientation.
    /// </summary>
    OneEightyDegreesState,

    /// <summary>
    /// The figure has been rotated clockwise by 270 degrees relative to its original orientation,
    /// which is equivalent to a 90-degree rotation in the opposite direction.
    /// </summary>
    TwoSeventyDegreesState
}
