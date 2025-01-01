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
        float targetGray = 30 / 255f; // Normalize 30 for RGB values to a range of 0 to 1

        float red = (float)(BaseColor.Red * Opacity + targetGray * grayBlend);
        float green = (float)(BaseColor.Green * Opacity + targetGray * grayBlend);
        float blue = (float)(BaseColor.Blue * Opacity + targetGray * grayBlend);

        // Clamp values to ensure they stay within the valid range [0, 1]
        red = Math.Clamp(red, 0, 1);
        green = Math.Clamp(green, 0, 1);
        blue = Math.Clamp(blue, 0, 1);

        return new Color(red, green, blue, 1.0f);
    }


}
