namespace Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;

public static class BoardElementActicationStrategyFactory {
    public static IBoardElementActivationStrategy GetActivationStrategy(string boardElementElementName) {
        return boardElementElementName switch {
            BoardElementFactory.GearName => new GearActivationStrategy(),
            BoardElementFactory.BlueConveyorBeltName => new BlueConveyorBeltActivationStrategy(),
            BoardElementFactory.GreenConveyorBeltName => new GreenConveyorBeltActivationStrategy(),

            _ => throw new ArgumentException("Invalid board element name")
        };
    }
}