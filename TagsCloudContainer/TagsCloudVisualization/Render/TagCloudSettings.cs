using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public class TagCloudSettings
{
    public Size ImageSize { get; set; } = new(1920, 1080);
    public Point CloudCenter { get; set; } = new(960, 540);
    public int MaxPointsPerRectangle { get; set; } = 50000;
    
    public Color BackgroundColor { get; set; } = Color.FromArgb(0, 34, 43);
    public Color TextColor { get; set; } = Color.FromArgb(212, 85, 0);
    public string FontName { get; set; } = "Arial";
    public FontStyle FontStyle { get; set; } = FontStyle.Bold;
    public float FontSizeMultiplier { get; set; } = 0.5f;
}