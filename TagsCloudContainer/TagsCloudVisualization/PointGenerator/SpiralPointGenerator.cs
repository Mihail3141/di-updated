using System.Drawing;

namespace TagsCloudVisualization.PointGenerator;

public class SpiralPointGenerator : IPointGenerator
{
    private readonly Point center;
    private readonly double radius;
    private readonly double angle;
    private readonly double ellipseRatioX;
    private readonly double ellipseRatioY;
    public SpiralPointGenerator(Point center, 
        double radius = 2, 
        double angle = double.Pi/40, 
        double ellipseRatioX = 1.5, 
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
        var currentRadius = 0.0;

        while (true)
        {
            var x = center.X + (int)Math.Round(Math.Cos(currentAngle) * currentRadius * ellipseRatioX);
            var y = center.Y + (int)Math.Round(Math.Sin(currentAngle) * currentRadius * ellipseRatioY);
            yield return new Point(x, y);
            currentAngle += angle;
            currentRadius += radius;
        }
    }
}