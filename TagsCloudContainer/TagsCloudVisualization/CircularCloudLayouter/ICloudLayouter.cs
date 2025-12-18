using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.CircularCloudLayouter;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}