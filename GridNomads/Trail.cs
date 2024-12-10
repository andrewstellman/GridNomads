
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
        Opacity -= 0.4; // Faster fading for trails
        if (Opacity < 0) Opacity = 0;
    }

    public Color GetFadedColor()
    {
        float grayBlend = 1 - (float)Opacity;
        float red = (float)(BaseColor.Red * Opacity + 0.3 * grayBlend); // Reduced color intensity
        float green = (float)(BaseColor.Green * Opacity + 0.3 * grayBlend);
        float blue = (float)(BaseColor.Blue * Opacity + 0.3 * grayBlend);

        return new Color(red, green, blue, 1.0f);
    }
}
