using System.Drawing;

namespace TagsCloudContainer.TagBuilder;

public class Tag(string word, Rectangle rect)
{
    public readonly Rectangle rect = rect;
    public readonly string word = word;
}