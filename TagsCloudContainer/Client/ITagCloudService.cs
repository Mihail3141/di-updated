using System.Drawing;
using TagsCloudContainer.TagsCloudVisualization.Render;

namespace TagsCloudContainer.Client;

public interface ITagCloudService
{
    public void GenerateTagCloud(string inputFile, string outputPath, TagCloudSettings options);
}