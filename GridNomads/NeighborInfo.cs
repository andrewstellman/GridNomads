namespace GridNomads;

public class NeighborInfo
{
    public required Direction Direction { get; set; }
    public required double Distance { get; set; }
    public required Color Color { get; set; } // Ensures initialization
}
