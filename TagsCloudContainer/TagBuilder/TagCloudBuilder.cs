using System.Drawing;
using System.Drawing.Text;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudVisualization.CircularCloudLayouter;

namespace TagsCloudContainer.TagBuilder;

public class TagCloudBuilder(ICloudLayouter layouter, TagCloudSettings settings) : ITagCloudBuilder
{
    public List<Tag> GetTagCloud(
        Dictionary<string, int> frequency,
        int? tagCount = null,
        int minFontSize = 10, 
        int maxFontSize = 60)
    {
        if (maxFontSize < minFontSize)
            throw new ArgumentException("maxFontSize must be greater than minFontSize");
        
        if (tagCount < 0)
            throw new ArgumentException("tagCount must be greater than or equal to zero");

        var sortedWords = frequency
            .OrderByDescending(p => p.Value)
            .Take(tagCount ?? frequency.Count)
            .ToList();

        var maxFreq = sortedWords.First().Value;
        var minFreq = sortedWords.Last().Value;

        var result = new List<Tag>();
        using var tmpBitmap = new Bitmap(1, 1);
        using var graphics = Graphics.FromImage(tmpBitmap);
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

        foreach (var wordFreq in sortedWords)
        {
            var normalizedFreq = (maxFreq - minFreq) > 0
                ? (double)(wordFreq.Value - minFreq) / (maxFreq - minFreq)
                : 0.0;

            var fontSize = (int)(minFontSize + normalizedFreq * (maxFontSize - minFontSize));
            using var font = new Font(settings.FontName, fontSize, FontStyle.Bold);
            var textSize = graphics.MeasureString(wordFreq.Key, font);

            var width = (int)Math.Ceiling(textSize.Width);
            var height = (int)Math.Ceiling(textSize.Height);

            var rect = layouter.PutNextRectangle(new Size(width, height));
            result.Add(new Tag(wordFreq.Key, rect));
        }

        return result;
    }
}