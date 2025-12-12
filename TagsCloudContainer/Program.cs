using System.Drawing;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudContainer.TextReader;
using TagsCloudContainer.WordProcess;
using TagsCloudVisualization;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainer;

public static class Program
{
    private static void Main()
    {
        var reader = new Reader();
        var wordProcessor = new WordProcessor();

        var sourceFilePath = Path.Combine("..", "..", "..", "SourceFile", "ГарриПоттерНаРусском.txt");
        
        var wordsFromText = reader.GetWordsFromFile(sourceFilePath);
        var processedWords = wordProcessor
            .SetAllowedPartsOfSpeech(["S","V"])
            .ExcludeWords(["быть"])
            .AddWordFilter(word => word.Lemma.Length > 2)
            .Get(wordsFromText);

        // var sourceFileDir = Path.GetDirectoryName(sourceFilePath) ?? "SourceFile";
        // var saver = new LemmasSaver(sourceFileDir);
        // saver.SaveLemmas(result);
        
        var frequency = processedWords
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());
        
        
        var center = new Point(1920 / 2, 1080 / 2);
        var pointGenerator = new SpiralPointGenerator(center, 1, double.Pi / 24);
        var layouter = new CircularCloudLayouter(center, 20000, pointGenerator);

        var builder = new TagCloudBuilder(layouter);
        var tags = builder.BuildTagCloud(100, frequency, 5, 120);

        var settings = new TagCloudRenderSettings
        {
            ImageSize = new Size(1920, 1080),
            FontName = "Arial",
            FontStyle = FontStyle.Italic,
            FontSizeMultiplier = 0.5f 
        };
        var renderer = new TagCloudRenderer(settings);

        using var bitmap = renderer.CreateRectangleCloud(tags);
        bitmap.Save("tagcloud.png", System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine("Облако сохранено: tagcloud.png");
    }
}