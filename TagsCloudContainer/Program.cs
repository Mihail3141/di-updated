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

        builder.Register(_ => new TagCloudSettings()).AsSelf().SingleInstance();
        
        builder.RegisterType<TextFileProcessor>().AsSelf().SingleInstance();

        builder.RegisterType<MyStemAnalyzer>().As<IStemAnalyzer>().SingleInstance();

        builder.RegisterType<WordProcessor>().AsSelf().SingleInstance();

        builder.RegisterType<SpiralPointGenerator>().As<IPointGenerator>().SingleInstance();

        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().SingleInstance();

        builder.RegisterType<TagCloudBuilder>().As<ITagCloudBuilder>().SingleInstance();
        
        builder.RegisterType<TagCloudRenderer>().As<ICloudRenderer>().SingleInstance();

        return builder.Build();
    }

    private static void Main()
    {
        using var container = CreateContainer();
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
        
        var frequency = processedWords
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());
        

        var tags = builder.GetTagCloud(frequency, 120, 10, 90);
        
        using var bitmap = renderer.CreateRectangleCloud(tags);
        
        var outputPath = Path.Combine(AppContext.BaseDirectory, "tagCloud.png");
        bitmap.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine($"Облако сохранено: {outputPath}");
    }
}