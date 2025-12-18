using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public class TagCloudRenderer(TagCloudSettings settings) : ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Tag> tags)
    {
        var bitmap = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);

        using var graphics = Graphics.FromImage(bitmap);

        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        
        using var bgBrush = new SolidBrush(settings.BackgroundColor);
        graphics.FillRectangle(bgBrush, 0, 0, settings.ImageSize.Width, settings.ImageSize.Height);
        float fontSize;
        foreach (var tag in tags)
        {
            fontSize = tag.rect.Height * settings.FontSizeMultiplier;
            using var font = new Font(settings.FontName, fontSize, settings.FontStyle);
            using var textBrush = new SolidBrush(settings.TextColor);
            graphics.DrawString(tag.word, font, textBrush, tag.rect.X, tag.rect.Y);
            // using var rectPen = new Pen(Color.FromArgb(120, 255, 255, 0), 1.5f);
            // graphics.DrawRectangle(rectPen, tag.rect);
        }

        return bitmap;
    }
}