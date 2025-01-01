namespace GridNomads;

/// <summary>
/// Represents a nomad on the grid with specific traits and behaviors.
/// </summary>
public class Nomad
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Color Color { get; }
    public PersonalityType Personality { get; }
    private List<NeighborInfo> neighbors = new();

    public double ExcitementLevel => neighbors.Count(n => n.Distance < 5) / 10.0;

    public Nomad(int row, int column, Color color)
    {
        Row = row;
        Column = column;
        Color = color;
        Personality = AssignPersonality(color);
    }

    /// <summary>
    /// Assigns a personality type based on the nomad's color.
    /// </summary>
    private PersonalityType AssignPersonality(Color color)
    {
        if (color == Colors.Crimson) return PersonalityType.Offensive;
        if (color == Colors.DodgerBlue) return PersonalityType.Defensive;
        throw new ArgumentException("Unsupported color for personality assignment.");
    }

    /// <summary>
    /// Moves the nomad randomly within the grid boundaries.
    /// </summary>
    public void Move(int maxRows, int maxCols, Random random)
    {
        var directions = new (int dRow, int dCol)[]
        {
            (-1, 0), (-1, 1), (0, 1), (1, 1),
            (1, 0), (1, -1), (0, -1), (-1, -1)
        };

        var (dRow, dCol) = directions[random.Next(directions.Length)];
        Row = (Row + dRow + maxRows) % maxRows;
        Column = (Column + dCol + maxCols) % maxCols;
    }

    /// <summary>
    /// Updates the list of neighboring nomads.
    /// </summary>
    public void UpdateNeighbors(List<NeighborInfo> newNeighbors)
    {
        neighbors = newNeighbors;
    }

    /// <summary>
    /// Calculates the distance to another nomad, accounting for edge wrapping.
    /// </summary>
    public double CalculateDistanceTo(Nomad other, int maxRows, int maxColumns)
    {
        int dRow = Math.Abs(Row - other.Row);
        int dCol = Math.Abs(Column - other.Column);

        dRow = Math.Min(dRow, maxRows - dRow);
        dCol = Math.Min(dCol, maxColumns - dCol);

        return Math.Sqrt(dRow * dRow + dCol * dCol);
    }

    /// <summary>
    /// Finds all nomads within a given range of this nomad.
    /// </summary>
    /// <param name="allNomads">The list of all nomads in the grid.</param>
    /// <param name="range">The proximity range to search within.</param>
    /// <param name="maxRows">The total number of rows in the grid.</param>
    /// <param name="maxColumns">The total number of columns in the grid.</param>
    /// <returns>A list of nearby nomads within the specified range.</returns>
    public List<Nomad> GetNearbyNomads(List<Nomad> allNomads, double range, int maxRows, int maxColumns)
    {
        return allNomads
            .Where(other => other != this && CalculateDistanceTo(other, maxRows, maxColumns) <= range)
            .ToList();
    }

    /// <summary>
    /// Performs an action based on the nomad's personality.
    /// </summary>
    public void Act()
    {
        switch (Personality)
        {
            case PersonalityType.Defensive:
                // Placeholder for future defensive behavior logic
                break;
            case PersonalityType.Offensive:
                // Placeholder for future offensive behavior logic
                break;
            default:
                throw new NotImplementedException($"Behavior for {Personality} not implemented.");
        }
    }
}
