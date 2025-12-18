using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainer.TagsCloudVisualization.PointGenerator;

public class SpiralPointGenerator : IPointGenerator
{
    private readonly Point center;
    private readonly double radius;
    private readonly double angle;
    private readonly double ellipseRatioX;
    private readonly double ellipseRatioY;
    public SpiralPointGenerator(Point center, 
        double radius = 1, 
        double angle = 0.1, 
        double ellipseRatioX = 2, 
        double ellipseRatioY = 1.0)
    {
        if (center.X < 0 || center.Y < 0)
            throw new ArgumentException("Center coordinates must be non-negative");
        if (radius <= 0)
            throw new ArgumentException("Radius must be positive");
        this.radius = radius;
        this.angle = angle;
        this.center = center;
        this.ellipseRatioX = ellipseRatioX;
        this.ellipseRatioY = ellipseRatioY;
    }

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