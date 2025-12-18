using System.Drawing;
using Autofac;
using TagsCloudContainer.Client;
using TagsCloudContainer.Lemmatization;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudContainer.TagsCloudVisualization.PointGenerator;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudContainer.TextReader;

namespace TagsCloudContainer;

public static class Program
{
    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();

        builder.Register(_ => new TagCloudSettings()).AsSelf().SingleInstance();
        
        builder.RegisterType<TxtReader>().As<ITextReader>().SingleInstance();
        
        builder.RegisterType<DocReader>().As<ITextReader>().SingleInstance();
        
        builder.RegisterType<TextFileProcessor>().AsSelf().SingleInstance();

        builder.RegisterType<MyStemAnalyzer>().As<IStemAnalyzer>().SingleInstance();

        builder.RegisterType<WordProcessor>().AsSelf().SingleInstance();

        builder.RegisterType<SpiralPointGenerator>().As<IPointGenerator>().SingleInstance();

        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().SingleInstance();

        builder.RegisterType<TagCloudBuilder>().As<ITagCloudBuilder>().SingleInstance();
        
        builder.RegisterType<TagCloudRenderer>().As<ICloudRenderer>().SingleInstance();
        
        builder.RegisterType<TagCloudService>().As<ITagCloudService>().SingleInstance();
        
        builder.RegisterType<ConsoleClient>().AsSelf().SingleInstance();

        return builder.Build();
    }

    private static int Main(string[] args)
    {
        using var container = CreateContainer();
        var client = container.Resolve<ConsoleClient>();
        return client.Run(args);
    }
}