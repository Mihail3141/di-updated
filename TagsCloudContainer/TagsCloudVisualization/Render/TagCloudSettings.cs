using System.Drawing;
using TagsCloudContainer.Lemmatization;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public class TagCloudSettings
{
    public string[] AllowedPartsOfSpeech { get; set; } = PartsOfSpeech.ContentWords.ToArray();
    public string[] ExcludeWords { get; set; } = [];
    public int MinWordLength { get; set; } = 2;
    public int TagsCount { get; set; } = 150;
    public int MinFontSize { get; set; } = 10;
    public int MaxFontSize { get; set; } = 90;
    public Size ImageSize { get; set; } = new(1920, 1080);
    public Point CloudCenter { get; set; } = new(960, 540);
    public int MaxPointsPerRectangle { get; set; } = 50000;
    public Color BackgroundColor { get; set; } = Color.FromArgb(0, 34, 43);
    public Color TextColor { get; set; } = Color.FromArgb(212, 85, 0);
    public string FontName { get; set; } = "Arial";
    public FontStyle FontStyle { get; set; } = FontStyle.Bold;
    public float FontSizeMultiplier { get; set; } = 0.5f;
}