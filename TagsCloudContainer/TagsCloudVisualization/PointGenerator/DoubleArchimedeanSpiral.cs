using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainer.TagsCloudVisualization.PointGenerator;

public class DoubleArchimedeanSpiral : IPointGenerator
{
    private readonly ArchimedeanSpiralPointGenerator _spiral1;
    private readonly ArchimedeanSpiralPointGenerator _spiral2;
    private Point _center;
    public DoubleArchimedeanSpiral(Point center)
    {
        _spiral1 = new ArchimedeanSpiralPointGenerator(center, 0.5, 1);
        var center2 = new Point(center.X+200, center.Y+100);
        _spiral2 = new ArchimedeanSpiralPointGenerator(center2, -0.5, 1);
        _center = center;
    }

    public IEnumerable<Point> GetPoints()
    {
        yield return _center;
        var enum1 = _spiral1.GetPoints().GetEnumerator();
        var enum2 = _spiral2.GetPoints().GetEnumerator();
        enum1.MoveNext();
        enum2.MoveNext();
        while (true)
        {
            yield return enum1.Current;
            yield return enum2.Current;
            
            enum1.MoveNext();
            enum2.MoveNext();
        }
    }
}