namespace TagsCloudContainer.TextReader;

public interface ITextReader
{
    List<string> ReadTextLines(string filePath);
}