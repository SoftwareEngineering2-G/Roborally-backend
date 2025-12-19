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

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 18" />
    public static BoardElement BlueConveyorBelt(Direction direction) {
        return new BlueConveyorBelt {
            Direction = direction
        };
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 24" />
    public static BoardElement BlueConveyorBelt(Direction direction, Direction[]? walls) {
        return new BlueConveyorBelt(walls) {
            Direction = direction
        };
    }

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 30" />
    public static BoardElement GreenConveyorBelt(Direction direction) {
        return new GreenConveyorBelt {
            Direction = direction
        };
    }
    
/// <author name="Sachin Baral 2025-10-20 21:20:17 +0200 36" />
    public static BoardElement GreenConveyorBelt(Direction direction, Direction[]? walls) {
        return new GreenConveyorBelt(walls) {
            Direction = direction
        };
    }

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 42" />
    public static BoardElement Gear(GearDirection direction) {
        return new Gear {
            Direction = direction
        };
    }
    
/// <author name="Nilanjana Devkota 2025-10-19 11:13:58 +0200 48" />
    public static BoardElement Gear(GearDirection direction, Direction[] walls) {
        return new Gear(walls) {
            Direction = direction
        };
    }

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 54" />
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

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 65" />
    public static bool IsThisElementLastInQueue(string boardElementName) {
        return boardElementName == ActivationOrder[^1];
    }
    
}