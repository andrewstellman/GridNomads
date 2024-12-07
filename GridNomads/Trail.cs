namespace GridNomads;

public class Trail
{
    public int Row { get; }
    public int Column { get; }
    public Color BaseColor { get; }
    public double Opacity { get; private set; } = 1.0;

    public Trail(int row, int column, Color baseColor)
    {
        Row = row;
        Column = column;
        BaseColor = baseColor;
    }

    public void Fade()
    {
        Opacity -= 0.25; // Fade increment
        if (Opacity < 0) Opacity = 0;
    }

    public Color GetFadedColor()
    {
        return new Color(BaseColor.Red, BaseColor.Green, BaseColor.Blue, (float)Opacity);
    }
}
