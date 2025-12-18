namespace TagsCloudContainer.TextReader;

public class TxtReader : ITextReader
{
    public List<string> ReadTextLines(string filePath) =>
        File.ReadAllLines(filePath).ToList();

    public bool CanRead(string pathToFile) =>
        Path.GetExtension(pathToFile).Equals(".txt", StringComparison.OrdinalIgnoreCase);
}