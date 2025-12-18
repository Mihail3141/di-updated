using System.Drawing;
using TagsCloudContainer.TagsCloudVisualization.Render;

namespace TagsCloudContainer.TagsCloudVisualization.PointGenerator;

public class SpiralPointGenerator(TagCloudSettings settings) : IPointGenerator
{
    private readonly Point center = settings.CloudCenter;
    private readonly double radius = 1;
    private readonly double angle = 0.1;
    private readonly double ellipseRatioX = 2;
    private readonly double ellipseRatioY = 1;

    public IEnumerable<Point> GetPoints()
    {
        yield return center;
        var currentAngle = 0.0;

        while (true)
        {
            var vector = radius * currentAngle/(2*double.Pi);
            var x = center.X + (int)Math.Round(vector * Math.Cos(currentAngle) * ellipseRatioX);
            var y =  center.Y + (int)Math.Round(vector * Math.Sin(currentAngle) * ellipseRatioY);
            yield return new Point(x, y);
            currentAngle += angle;
        }
    }
}