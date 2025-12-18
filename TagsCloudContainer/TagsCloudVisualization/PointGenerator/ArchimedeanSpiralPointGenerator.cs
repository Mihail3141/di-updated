using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.PointGenerator;

public class ArchimedeanSpiralPointGenerator : IPointGenerator
{
    private readonly Point _center;
    private readonly double _angleStep;
    private readonly double _radiusStep;
    private double _angle;

    public ArchimedeanSpiralPointGenerator(
        Point center,
        double angleStep = 0.05,
        double radiusStep = 1,
        double angle = 0)
    {
        _center = center;
        _angleStep = angleStep;
        _radiusStep = radiusStep;
        _angle = angle;
    }

    public IEnumerable<Point> GetPoints()
    {
        yield return _center;
        
        while (true)
        {
            var radius = _radiusStep * _angle;
            var x = _center.X + (int)Math.Round(radius * Math.Cos(_angle));
            var y = _center.Y + (int)Math.Round(radius * Math.Sin(_angle));
            
            yield return new Point(x, y);
            _angle += _angleStep;
        }
    }
}