using System.Reflection;
using CommandLine;
using TagsCloudContainer.TagsCloudVisualization.Render;

namespace TagsCloudContainer.Client;

public class ConsoleClient(ITagCloudService service)
{
    public int Run(string[] args)
    {
        return Parser.Default
            .ParseArguments<GenerateOptions>(args)
            .MapResult(RunGenerate, _ => 1);
    }

    private int RunGenerate(GenerateOptions options)
    {
        var inputFile = Path.Combine("..", "..", "..", "SourceFile", "ГарриПоттерНаРусском.txt");
        
        var settings = new TagCloudSettings
        {
            
            TagsCount = options.TagCount,
            MinFontSize = options.MinFontSize,
            MaxFontSize = options.MaxFontSize,
            ExcludeWords = options.ExcludeWords.ToArray(),
            MinWordLength = options.MinWordLength
        };

        try
        {
            var fullOutputPath = Path.GetFullPath(options.OutputFilePath);
            service.GenerateTagCloud(inputFile, fullOutputPath, settings);
        
            if (File.Exists(fullOutputPath))
            {
                Console.WriteLine($"СОЗДАНО: {fullOutputPath}");
                return 0;
            }
            Console.WriteLine($"НЕ СОЗДАНО: {fullOutputPath}");
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}