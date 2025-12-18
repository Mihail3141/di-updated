using System.Drawing;
using FluentAssertions;
using TagsCloudContainer;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.PointGenerator;
using TagsCloudVisualization.CircularCloudLayouter;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudContainerTest;

public class TagCloudBuilderTest
{
    private Dictionary<string, int> _frequency;
    private TagCloudBuilder _builder;
    private ICloudLayouter _layouter;

    [SetUp]
    public void Setup()
    {
        var center = new Point(1920 / 2, 1080 / 2);
        _layouter = new CircularCloudLayouter(center, 20000, new SpiralPointGenerator(center));
        _frequency = new Dictionary<string, int>
        {
            ["хогвартс"] = 500,
            ["гарри"] = 1400,
            ["волдеморт"] = 300,
            ["поттер"] = 800,
            ["гермиона"] = 200,
            ["рон"] = 200
        };
        _builder = new TagCloudBuilder(_layouter);
    }

    [Test]
    public void BuildTagCloud_ShouldReturnCorrectCount_WhenValidParameters()
    {
        var result = _builder.BuildTagCloud(3, _frequency);

        result.Should().HaveCount(3);
        result[0].word.Should().Be("гарри");
        result[1].word.Should().Be("поттер");
        result[2].word.Should().Be("хогвартс");
    }

    [Test]
    public void BuildTagCloud_ShouldThrowArgumentException_WhenMaxFontSizeLessThanMin()
    {
        var act = () => _builder.BuildTagCloud(3, _frequency, 20, 10);

        act.Should().Throw<ArgumentException>()
            .WithMessage("maxFontSize must be greater than minFontSize");
    }

    [Test]
    public void BuildTagCloud_ShouldThrowArgumentException_WhenCountLessZero()
    {
        var act = () => _builder.BuildTagCloud(-1, _frequency, 10, 100);

        act.Should().Throw<ArgumentException>()
            .WithMessage("tagCount must be greater than zero");
    }

    [Test]
    public void BuildTagCloud_ShouldHaveLargerRectanglesForMoreFrequentWords_WhenDifferentFrequencies()
    {
        var result = _builder.BuildTagCloud(3, _frequency, 10, 100);

        var size1 = result[0].rect.Size;
        var size2 = result[1].rect.Size;

        size1.Width.Should().BeGreaterThan(size2.Width);
        size1.Height.Should().BeGreaterThan(size2.Height);
    }


    [Test]
    public void BuildTagCloud_ShouldEqualFontSize_WhenAllFrequenciesAreEqual()
    {
        var sameFreq = new Dictionary<string, int> { ["слово1"] = 100, ["слово2"] = 100 };

        var result = _builder.BuildTagCloud(2, sameFreq, 10, 500);

        result[0].rect.Width.Should().Be(result[1].rect.Width);
        result[0].rect.Height.Should().Be(result[1].rect.Height);
    }

    [Test]
    public void BuildTagCloud_ShouldHaveLargerWidthForLongerWords_WhenSameFrequency()
    {
        var sameFreq = new Dictionary<string, int> { ["короткое"] = 100, ["очень_длинное_слово"] = 100 };

        var result = _builder.BuildTagCloud(2, sameFreq, 10, 500);

        var shortWidth = result[0].rect.Width;
        var longWidth = result[1].rect.Width;

        longWidth.Should().BeGreaterThan(shortWidth);
        result[0].rect.Height.Should().Be(result[1].rect.Height);
    }
}