namespace Roborally.core.domain.Game.Gameboard.Space;

public abstract class Space {
    public abstract string Name();

// so tiles can react when entered
public virtual void OnEnter(Roborally.core.domain.Game.Player.Player player) { }
}
