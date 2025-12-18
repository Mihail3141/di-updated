using System.Drawing;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainer;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectangles = [];

    private readonly IPointGenerator pointGenerator;

    private readonly int maxPointsPerRectangle;

    public CircularCloudLayouter(TagCloudSettings settings, IPointGenerator pointGenerator)
    {
        ArgumentNullException.ThrowIfNull(pointGenerator);
        ArgumentNullException.ThrowIfNull(settings);


        maxPointsPerRectangle = settings.MaxPointsPerRectangle;
        this.pointGenerator = pointGenerator;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = pointGenerator
            .GetPoints()
            .Take(maxPointsPerRectangle);


        foreach (var point in points)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            for (var i = rectangles.Count - 1; i >= 0; i--)
            {
                if (rectangles[i].IntersectsWith(rectangle))
                    goto NextPoint;
            }

            rectangles.Add(rectangle);
            return rectangle;
            NextPoint: ;
        }

        throw new ArgumentException($"Failed to find place for the {rectangles.Count} rectangle");
    }
}