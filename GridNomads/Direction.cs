namespace GridNomads;

public enum Direction
{
    N, NE, E, SE, S, SW, W, NW
}

public static class DirectionHelper
{
    public static Direction GetDirection(int dRow, int dCol)
    {
        return (dRow, dCol) switch
        {
            (-1, 0) => Direction.N,
            (-1, 1) => Direction.NE,
            (0, 1) => Direction.E,
            (1, 1) => Direction.SE,
            (1, 0) => Direction.S,
            (1, -1) => Direction.SW,
            (0, -1) => Direction.W,
            (-1, -1) => Direction.NW,
            _ => throw new ArgumentException("Invalid direction"),
        };
    }
}
