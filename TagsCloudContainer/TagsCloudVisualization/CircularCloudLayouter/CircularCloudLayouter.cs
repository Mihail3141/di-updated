using System.Drawing;
using TagsCloudContainer.TagsCloudVisualization.PointGenerator;
using TagsCloudContainer.TagsCloudVisualization.Render;

namespace TagsCloudContainer.TagsCloudVisualization.CircularCloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> _rectangles = [];

    private readonly IPointGenerator _pointGenerator;

    private readonly int _maxPointsPerRectangle;

    private Rectangle _candidateRect;

    public CircularCloudLayouter(TagCloudSettings settings, IPointGenerator pointGenerator)
    {
        ArgumentNullException.ThrowIfNull(pointGenerator);
        ArgumentNullException.ThrowIfNull(settings);

        _maxPointsPerRectangle = settings.MaxPointsPerRectangle;
        _pointGenerator = pointGenerator;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = _pointGenerator
            .GetPoints()
            .Take(_maxPointsPerRectangle);


        foreach (var point in points)
        {
            _candidateRect.Location = point;
            _candidateRect.Size = rectangleSize;

            var hasIntersects = false;
            for (var i = _rectangles.Count - 1; i >= 0; i--)
            {
                if (!_rectangles[i].IntersectsWith(_candidateRect))
                    continue;
                hasIntersects = true;
                break;
            }

            if (hasIntersects)
                continue;

            var findRect = new Rectangle(point, rectangleSize);
            _rectangles.Add(findRect);
            return findRect;
        }

        throw new ArgumentException($"Failed to find place for the {_rectangles.Count} rectangle");
    }
}