using System.Drawing;

namespace TagsCloudContainer.TagBuilder;

public interface ITagCloudBuilder
{
    public List<Tag> BuildTagCloud(int tagCount, Dictionary<string, int> frequency,
        int minFontSize, int maxFontSize);
}