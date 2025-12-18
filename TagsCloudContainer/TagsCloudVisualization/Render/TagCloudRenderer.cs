using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public class TagCloudRenderer : ICloudRenderer
{
    private readonly TagCloudSettings _settings;

    public TagCloudRenderer(TagCloudSettings settings)
    {
        _settings = settings;
    }
    public Bitmap CreateRectangleCloud(IEnumerable<Tag> tags)
    {
        var bitmap = new Bitmap(_settings.ImageSize.Width, _settings.ImageSize.Height);

        using var graphics = Graphics.FromImage(bitmap);

        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        
        using var bgBrush = new SolidBrush(_settings.BackgroundColor);
        graphics.FillRectangle(bgBrush, 0, 0, _settings.ImageSize.Width, _settings.ImageSize.Height);
        float fontSize;
        foreach (var tag in tags)
        {
            fontSize = tag.rect.Height * _settings.FontSizeMultiplier;
            using var font = new Font(_settings.FontName, fontSize, _settings.FontStyle);
            using var textBrush = new SolidBrush(_settings.TextColor);
            graphics.DrawString(tag.word, font, textBrush, tag.rect.X, tag.rect.Y);
            using var rectPen = new Pen(Color.FromArgb(120, 255, 255, 0), 1.5f);
            graphics.DrawRectangle(rectPen, tag.rect);
        }

        return bitmap;
    }
}