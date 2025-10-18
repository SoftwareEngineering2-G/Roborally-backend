using System;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public static class BoardElementFactory {
    public const string BlueConveyorBeltName = "BlueConveyorBelt";
    public const string GreenConveyorBeltName = "GreenConveyorBelt";
    public const string GearName = "Gear";

    // Use Type objects for activation order
    private static readonly string[] ActivationOrder = new string[] {
        BlueConveyorBeltName,
        GreenConveyorBeltName,
        GearName
    };

    public static BoardElement BlueConveyorBelt(Direction direction, Direction[]? walls = null) {
        return new BlueConveyorBelt(walls) {
            Direction = direction
        };
    }

    public static BoardElement GreenConveyorBelt(Direction direction, Direction[]? walls = null) {
        return new GreenConveyorBelt(walls) {
            Direction = direction
        };
    }

    public static BoardElement Gear(GearDirection direction, Direction[]? walls = null) {
        return new Gear(walls) {
            Direction = direction
        };
    }

    public static string GetNextForActivation(string? lastActivatedElement) {
        if (string.IsNullOrEmpty(lastActivatedElement)) {
            return ActivationOrder[0];
        }
        int idx = Array.IndexOf(ActivationOrder, lastActivatedElement);
        if (idx == -1) {
            return ActivationOrder[0];
        }
        return ActivationOrder[(idx + 1) % ActivationOrder.Length];
    }
}