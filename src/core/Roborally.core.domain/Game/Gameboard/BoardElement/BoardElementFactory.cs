using System;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public static class BoardElementFactory {
    internal const string BlueConveyorBeltName = "BlueConveyorBelt";
    internal const string GreenConveyorBeltName = "GreenConveyorBelt";
    internal const string GearName = "Gear";

    // Use Type objects for activation order
    private static readonly string[] ActivationOrder = new string[] {
        BlueConveyorBeltName,
        GreenConveyorBeltName,
        GearName
    };

    public static BoardElement BlueConveyorBelt(Direction direction) {
        return new BlueConveyorBelt {
            Direction = direction
        };
    }
    
    public static BoardElement BlueConveyorBelt(Direction direction, Direction[]? walls) {
        return new BlueConveyorBelt(walls) {
            Direction = direction
        };
    }

    public static BoardElement GreenConveyorBelt(Direction direction) {
        return new GreenConveyorBelt {
            Direction = direction
        };
    }
    
    public static BoardElement GreenConveyorBelt(Direction direction, Direction[]? walls) {
        return new GreenConveyorBelt(walls) {
            Direction = direction
        };
    }

    public static BoardElement Gear(GearDirection direction) {
        return new Gear {
            Direction = direction
        };
    }
    
    public static BoardElement Gear(GearDirection direction, Direction[] walls) {
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

    public static bool IsThisElementLastInQueue(string boardElementName) {
        return boardElementName == ActivationOrder[^1];
    }
    
}