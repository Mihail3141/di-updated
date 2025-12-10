using Spire.Doc;


namespace TagsCloudContainer.TextReader;

public class DocReader : ITextReader
{
    public List<string> ReadTextLines(string filePath)
    {
        var document = new Document();
        document.LoadFromFile(filePath);

        return document
            .GetText()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToList();
    }
}