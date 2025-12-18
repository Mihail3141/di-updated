using System.Text.RegularExpressions;

namespace TagsCloudContainer.TextReader;

public partial class TextFileProcessor(IEnumerable<ITextReader> textReaders)
{
    private static readonly char[] WordDelimiters =
    [
        ' ', '\t', '\n', '\r', '\v', '\f', ',', '.', '!',
        ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}',
        '-', '–', '—', '/', '\\'
    ];

    private static readonly Regex WordNormalizer = MyRegex();

    public List<string> GetWordsFromFile(string pathToFile)
    {
        if (!File.Exists(pathToFile))
            throw new FileNotFoundException("File not found", pathToFile);

        var reader = textReaders.FirstOrDefault(r => r.CanRead(pathToFile))
                     ?? throw new FileNotFoundException($"No reader for {Path.GetExtension(pathToFile)}");

        var lines = reader.ReadTextLines(pathToFile);

        return lines
            .SelectMany(line => line.Split(WordDelimiters, StringSplitOptions.RemoveEmptyEntries))
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => WordNormalizer.Replace(word, "").ToLowerInvariant())
            .ToList();
    }
    
    [GeneratedRegex(@"[^\p{L}]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}