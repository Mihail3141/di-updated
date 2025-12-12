using System.Drawing;
using TagsCloudVisualization.CircularCloudLayouter;

namespace TagsCloudContainer.TagBuilder;

public class TagCloudBuilder(ICloudLayouter layouter) : ITagCloudBuilder
{
    public List<(Rectangle rect, string word)> BuildTagCloud(int tagCount, Dictionary<string, int> frequency,
        int minFontSize = 10, int maxFontSize = 60)
    {
        if (maxFontSize < minFontSize)
            throw new ArgumentException("maxFontSize must be greater than minFontSize");
        if (tagCount < 0)
            throw new ArgumentException("tagCount must be greater than zero");

        var sortedWords = frequency
            .OrderByDescending(p => p.Value)
            .Take(tagCount)
            .ToList();

        var maxFreq = sortedWords.First().Value;
        var minFreq = sortedWords.Last().Value;

        var result = new List<(Rectangle, string)>();

        foreach (var wordFreq in sortedWords)
        {
            var normalizedFreq = (maxFreq - minFreq) > 0 
                ? (double)(wordFreq.Value - minFreq) / (maxFreq - minFreq)
                : 0.0;

            var fontSize = (int)(minFontSize + normalizedFreq * (maxFontSize - minFontSize));

            var width = wordFreq.Key.Length * fontSize / 2 + fontSize;
            var height = (int)(fontSize * 1.7);

            var rect = layouter.PutNextRectangle(new Size(width, height));
            result.Add((rect, wordFreq.Key));
        }

        return result;
    }
}