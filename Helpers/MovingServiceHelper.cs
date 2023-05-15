using System;
using TetrisConsole.Enums;
using TetrisConsole.Models;

namespace TetrisConsole.Helpers;

public static class MovingServiceHelper
{
    /// <summary>
    /// Returns the next rotation state of the figure.
    /// </summary>
    /// <param name="figure">Active figure.</param>
    /// <returns></returns>
    public static RotationStateTypeEnum GetNextRotationState(FigureModel figure)
    {
        var rotationStates = Enum.GetValues<RotationStateTypeEnum>().Skip(1).ToList();

        int index = rotationStates.IndexOf(figure.Angle);

        if (++index == rotationStates.Count) index = 0;

        return rotationStates[index];
    }
}

