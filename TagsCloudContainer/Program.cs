using System.Drawing;
using TagsCloudContainer.TagBuilder;
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
        //var wordProcessor = new WordProcessor(Path.Combine(AppContext.BaseDirectory, "mystem.exe"));

        var sourceFilePath = Path.Combine("..", "..", "..", "SourceFile", "lemmas.txt");

        var words = reader.GetWordsFromFile(sourceFilePath);

        var frequency = words
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());

        //var result = wordProcessor.ProcessWords(words);
        // var sourceFileDir = Path.GetDirectoryName(sourceFilePath) ?? "SourceFile";
        // var saver = new LemmasSaver(sourceFileDir);
        // saver.SaveLemmas(result);
        // var frequency = result
        //     .GroupBy(w => w)
        //     .ToDictionary(g => g.Key, g => g.Count());
        //

        var center = new Point(1920 / 2, 1080 / 2);
        var pointGenerator = new SpiralPointGenerator(center, 1.1, double.Pi / 30);
        var layouter = new CircularCloudLayouter(center, 20000, pointGenerator);

        var builder = new TagCloudBuilder(layouter);
        var taggedRects = builder.BuildTagCloud(120, frequency, 15, 160);


        var renderer = new TagCloudRenderer(new Size(1920, 1080));

        using var bitmap = renderer.CreateRectangleCloud(taggedRects);
        bitmap.Save("tagcloud.png", System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine("Облако сохранено: tagcloud.png");
    }
}