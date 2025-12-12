using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace TagsCloudVisualization;

public class TagCloudRenderer(Size imageSize) 
{
    private readonly Brush _backgroundBrush = new SolidBrush(Color.FromArgb(0, 34, 43));
    private readonly Brush _textBrush = new SolidBrush(Color.FromArgb(212, 85, 0)); 

    public Bitmap CreateRectangleCloud(IEnumerable<(Rectangle rect, string word)> taggedRectangles)
    {
        var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        
        using var graphics = Graphics.FromImage(bitmap);
        
  
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        

        graphics.FillRectangle(_backgroundBrush, 0, 0, imageSize.Width, imageSize.Height);
        
        foreach (var (rect, word) in taggedRectangles)
        {
   
            var fontSize = Math.Max(10, rect.Height / 2);
            var font = new Font("Segoe UI", fontSize, FontStyle.Bold);
            
  
            var stringSize = graphics.MeasureString(word, font);
            var textX = rect.X + (rect.Width - stringSize.Width) / 2;
            var textY = rect.Y + (rect.Height - stringSize.Height) / 2;
            

            graphics.DrawString(word, font, _textBrush, textX, textY);
            
            font.Dispose();
        }
        
        return bitmap;
    }
}