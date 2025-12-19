namespace Roborally.core.domain.Game.Player;

public class Position {
/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 4" />
    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

/// <author name="nilanjanadevkota 2025-10-14 19:37:00 +0200 12" />
    public Position GetNext(Direction direction) {
        return direction.GetNextPosition(this);
    }

/// <author name="Vincenzo Altaserse 2025-10-20 10:08:18 +0200 16" />
    public override bool Equals(object? obj)
    {
        if (obj is not Position otherValue)
        {
            return false;
        }
        
        return X == otherValue.X && Y == otherValue.Y;
    }
    
/// <author name="Vincenzo Altaserse 2025-10-20 10:08:18 +0200 26" />
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
};