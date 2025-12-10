using TagsCloudContainer.TextReader;
using TagsCloudContainer.WordProcess;

namespace TagsCloudContainer;

public static class Program
{
    private static void Main()
    {
        var reader = new Reader();
        var wordProcessor = new WordProcessor(Path.Combine(AppContext.BaseDirectory, "mystem.exe"));
        
        var sourceFilePath = Path.Combine("..", "..", "..", "SourceFile", "ГарриПоттерНаРусском.txt");

        var words = reader.GetWordsFromFile(sourceFilePath);
        var result = wordProcessor.ProcessWords(words);
        
        var sourceFileDir = Path.GetDirectoryName(sourceFilePath) ?? "SourceFile";
        var saver = new LemmasSaver(sourceFileDir);
        saver.SaveLemmas(result);
    }
}