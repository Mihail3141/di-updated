using System.Text;

namespace TagsCloudContainer;

public class LemmasSaver
{
    private readonly string _outputDirectory;

    public LemmasSaver(string outputDirectory)
    {
        _outputDirectory = outputDirectory ?? throw new ArgumentNullException(nameof(outputDirectory));
        Directory.CreateDirectory(_outputDirectory); 
    }
    
    public void SaveLemmas(List<string> lemmas, string outputFileName = "lemmas.txt")
    {
        var outputPath = Path.Combine(_outputDirectory, outputFileName);
        
        File.WriteAllLines(outputPath, lemmas, Encoding.UTF8);
        
        Console.WriteLine($"Сохранено {lemmas.Count} лемм в {outputPath}");
    }
}