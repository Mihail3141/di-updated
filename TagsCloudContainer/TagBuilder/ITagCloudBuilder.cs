namespace TagsCloudContainer.TagBuilder;

public interface ITagCloudBuilder
{
    public List<Tag> GetTagCloud(
        Dictionary<string, int> frequency,
        int? tagCount,
        int minFontSize,
        int maxFontSize);
}