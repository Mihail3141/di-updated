using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Render;

public interface ICloudRenderer
{
    public Bitmap CreateRectangleCloud(IEnumerable<Tag> tags);
}