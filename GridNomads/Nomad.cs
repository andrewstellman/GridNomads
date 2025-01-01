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
    private const double AvoidanceRange = 3.0; // Proximity range for avoidance

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
    /// Performs an action based on the nomad's personality.
    /// </summary>
    /// <param name="allNomads">The list of all nomads in the grid.</param>
    /// <param name="maxRows">The total number of rows in the grid.</param>
    /// <param name="maxCols">The total number of columns in the grid.</param>
    /// <param name="random">A random number generator for movement decisions.</param>
    public void Act(List<Nomad> allNomads, int maxRows, int maxCols, Random random)
    {
        switch (Personality)
        {
            case PersonalityType.Defensive:
                PerformDefensiveAction(allNomads, maxRows, maxCols, random);
                break;
            case PersonalityType.Offensive:
                // Placeholder for offensive behavior logic
                break;
            default:
                throw new NotImplementedException($"Behavior for {Personality} not implemented.");
        }
    }

    /// <summary>
    /// Executes the defensive behavior to avoid red nomads within range.
    /// </summary>
    private void PerformDefensiveAction(List<Nomad> allNomads, int maxRows, int maxCols, Random random)
    {
        var nearbyRedNomads = allNomads
            .Where(n => n.Personality == PersonalityType.Offensive &&
                        CalculateDistanceTo(n, maxRows, maxCols) <= AvoidanceRange)
            .ToList();

        if (!nearbyRedNomads.Any())
        {
            // No nearby red nomads, perform random movement
            Move(maxRows, maxCols, random);
            return;
        }

        // Calculate the direction to move away from red nomads
        var directions = new (int dRow, int dCol)[]
        {
            (-1, 0), (-1, 1), (0, 1), (1, 1),
            (1, 0), (1, -1), (0, -1), (-1, -1)
        };

        var bestDirection = directions
            .Select(dir => new
            {
                Direction = dir,
                DistanceSum = nearbyRedNomads.Sum(n =>
                {
                    int newRow = (Row + dir.dRow + maxRows) % maxRows;
                    int newCol = (Column + dir.dCol + maxCols) % maxCols;
                    return Math.Sqrt(Math.Pow(newRow - n.Row, 2) + Math.Pow(newCol - n.Column, 2));
                })
            })
            .OrderByDescending(result => result.DistanceSum)
            .FirstOrDefault();

        if (bestDirection != null && bestDirection.DistanceSum > 0)
        {
            // Move in the direction that maximizes distance from red nomads
            Row = (Row + bestDirection.Direction.dRow + maxRows) % maxRows;
            Column = (Column + bestDirection.Direction.dCol + maxCols) % maxCols;
        }
        // Else, stay in place if no beneficial move is found
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
}
