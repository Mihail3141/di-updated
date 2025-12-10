namespace TagsCloudContainer.TextReader;

public class TxtReader : ITextReader
{
    public List<string> ReadTextLines(string filePath) =>
        File.ReadAllLines(filePath).ToList();
}