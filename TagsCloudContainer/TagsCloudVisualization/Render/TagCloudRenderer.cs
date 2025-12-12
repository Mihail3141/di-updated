using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public class TagCloudRenderer : ICloudRenderer
{
    private readonly TagCloudRenderSettings _settings;

    public TagCloudRenderer(TagCloudRenderSettings settings)
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

        foreach (var tag in tags)
        {
            var fontSize = tag.rect.Height * _settings.FontSizeMultiplier;
            var font = new Font(_settings.FontName, fontSize, _settings.FontStyle);
            
            var textSize = graphics.MeasureString(tag.word, font);
            var textX = tag.rect.X + (tag.rect.Width - textSize.Width) / 2;
            var textY = tag.rect.Y + (tag.rect.Height - textSize.Height) / 2;
            
            using var textBrush = new SolidBrush(_settings.TextColor);
            graphics.DrawString(tag.word, font, textBrush, textX, textY);
            font.Dispose();
        }

        return bitmap;
    }
}