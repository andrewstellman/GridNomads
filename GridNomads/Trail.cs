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

    public void Fade(double amount = 0.05)
    {
        Opacity -= amount; // Smoother fading
        if (Opacity < 0) Opacity = 0;
    }

    public Color GetFadedColor()
    {
        float grayBlend = 1 - (float)Opacity;
        float targetGray = 0.12f; // RGB value for Color.FromRgb(30, 30, 30)

        float red = (float)(BaseColor.Red * Opacity + targetGray * grayBlend);
        float green = (float)(BaseColor.Green * Opacity + targetGray * grayBlend);
        float blue = (float)(BaseColor.Blue * Opacity + targetGray * grayBlend);

        return new Color(red, green, blue, 1.0f);
    }

}
