namespace GridNomads;

public static class DirectionHelper
{
    public static Direction GetDirection(int dRow, int dCol)
    {
        // Normalize (dRow, dCol) to one of the cardinal/diagonal directions
        var directions = new (int dRow, int dCol, Direction direction)[]
        {
            (-1, 0, Direction.N),
            (-1, 1, Direction.NE),
            (0, 1, Direction.E),
            (1, 1, Direction.SE),
            (1, 0, Direction.S),
            (1, -1, Direction.SW),
            (0, -1, Direction.W),
            (-1, -1, Direction.NW)
        };

        var normalized = directions
            .OrderBy(d => Math.Sqrt(Math.Pow(dRow - d.dRow, 2) + Math.Pow(dCol - d.dCol, 2)))
            .First();

        return normalized.direction;
    }
}
