namespace GridNomads;

public class Nomad
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Color Color { get; }
    private List<NeighborInfo> neighbors = new();

    public double ExcitementLevel => neighbors.Count(n => n.Distance < 5) / 10.0;

    public Nomad(int row, int column, Color color)
    {
        Row = row;
        Column = column;
        Color = color;
    }

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

    public void UpdateNeighbors(List<NeighborInfo> newNeighbors)
    {
        neighbors = newNeighbors;
    }
}
