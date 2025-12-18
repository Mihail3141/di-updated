using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.PointGenerator;

public interface IPointGenerator
{
    public IEnumerable<Point> GetPoints();
}