namespace Roborally.core.domain.Game.Player;

public class Position {
    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public Position GetNext(Direction direction) {
        return direction.GetNextPosition(this);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Position otherValue)
        {
            return false;
        }
        
        return X == otherValue.X && Y == otherValue.Y;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
};