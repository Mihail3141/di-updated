using System.Drawing;
using TagsCloudContainer.TagBuilder;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public interface ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Tag> tags);
}