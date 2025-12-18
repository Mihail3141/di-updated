using System.Text.RegularExpressions;

namespace TagsCloudContainer.TextReader;

public partial class TextFileProcessor
{
    private static readonly char[] WordDelimiters =
    [
        ' ', '\t', '\n', '\r', '\v', '\f', ',', '.', '!',
        ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}',
        '-', '–', '—', '/', '\\'
    ];

    private static readonly Dictionary<string, ITextReader> TextReaders = new(StringComparer.OrdinalIgnoreCase)
    {
        [".txt"] = new TxtReader(),
        [".doc"] = new DocReader(),
        [".docx"] = new DocReader(),
    };
    
    private static readonly Regex WordNormalizer = MyRegex();

    public List<string> GetWordsFromFile(string pathToFile)
    {
        if (!File.Exists(pathToFile))
            throw new FileNotFoundException("File not found", pathToFile);

        var extension = Path.GetExtension(pathToFile);
        if (!TextReaders.TryGetValue(extension, out var reader))
            throw new FileNotFoundException($"File format {extension} is not specified", pathToFile);

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