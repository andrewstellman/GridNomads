namespace GridNomads;

/// <summary>
/// Provides utility methods for working with directions.
/// </summary>
public static class DirectionHelper
{
    /// <summary>
    /// Determines the closest cardinal or diagonal direction based on the given row and column offsets.
    /// </summary>
    /// <param name="dRow">The row offset.</param>
    /// <param name="dCol">The column offset.</param>
    /// <returns>The <see cref="Direction"/> corresponding to the closest match.</returns>
    /// <remarks>
    /// This method uses the Euclidean distance formula (involving <see cref="Math.Sqrt"/> and <see cref="Math.Pow"/>)
    /// to calculate how close the given offsets are to each of the predefined directions.
    /// If the offsets don't directly match any predefined direction, the closest one is selected.
    /// </remarks>
    public static Direction GetDirection(int dRow, int dCol)
    {
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
