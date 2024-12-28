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
