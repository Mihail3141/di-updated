using System.Drawing;
using Autofac;
using TagsCloudContainer.Lemmatization;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.PointGenerator;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudContainer.TextReader;
using TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainer;

public static class Program
{
    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        var center = new Point(1920 / 2, 1080 / 2);
        builder.RegisterType<TextFileProcessor>().AsSelf().SingleInstance();
        builder.RegisterType<MyStemAnalyzer>().As<IStemAnalyzer>().SingleInstance();
        builder.Register(ctx => new WordProcessor(ctx.Resolve<IStemAnalyzer>()));
        builder.Register(ctx =>
            new SpiralPointGenerator(new Point(700, 400)));
        builder.Register(ctx =>
                new CircularCloudLayouter(center, 50000, ctx.Resolve<SpiralPointGenerator>()))
            .As<ICloudLayouter>().SingleInstance();
        builder.Register(ctx => new TagCloudBuilder(ctx.Resolve<ICloudLayouter>()))
            .As<ITagCloudBuilder>().SingleInstance();
        builder.Register(ctx => new TagCloudRenderSettings
            {
                ImageSize = new Size(1920, 1080),
                FontName = "Times New Roman",
                FontStyle = FontStyle.Bold,
                FontSizeMultiplier = 0.6f
            })
            .AsSelf().SingleInstance();
        builder.Register(ctx =>
                new TagCloudRenderer(ctx.Resolve<TagCloudRenderSettings>()))
            .As<ICloudRenderer>().SingleInstance();

        return builder.Build();
    }

    private static void Main()
    {
        var container = CreateContainer();
        using var scope = container.BeginLifetimeScope();
        
        var reader = scope.Resolve<TextFileProcessor>();
        var wordProcessor = scope.Resolve<WordProcessor>();
        var builder = scope.Resolve<ITagCloudBuilder>();
        var renderer = scope.Resolve<ICloudRenderer>();

        var sourceFilePath = Path.Combine("..", "..", "..", "SourceFile", "ГарриПоттерНаРусском.txt");

        var wordsFromText = reader.GetWordsFromFile(sourceFilePath);
        var processedWords = wordProcessor
            .SetAllowedPartsOfSpeech([PartsOfSpeech.Noun, PartsOfSpeech.Verb])
            .ExcludeWords(["быть"])
            .AddWordFilter(word => word.Lemma.Length > 2)
            .Get(wordsFromText);

        // var sourceFileDir = Path.GetDirectoryName(sourceFilePath) ?? "SourceFile";
        // var saver = new LemmasSaver(sourceFileDir);
        // saver.SaveLemmas(result);

        var frequency = processedWords
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());
        
        var tags = builder.BuildTagCloud(90, frequency, 20, 120);
        
        using var bitmap = renderer.CreateRectangleCloud(tags);
        bitmap.Save("tagcloud.png", System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine("Облако сохранено: tagcloud.png");
    }
}