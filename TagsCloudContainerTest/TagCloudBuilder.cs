using System.Drawing;
using FluentAssertions;
using TagsCloudContainer;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.PointGenerator;
using TagsCloudContainer.TagsCloudVisualization.Render;
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
        _layouter = new CircularCloudLayouter(new TagCloudSettings(), new SpiralPointGenerator(new TagCloudSettings()));
        _frequency = new Dictionary<string, int>
        {
            ["хогвартс"] = 500,
            ["гарри"] = 1400,
            ["волдеморт"] = 300,
            ["поттер"] = 800,
            ["гермиона"] = 200,
            ["рон"] = 200
        };
        _builder = new TagCloudBuilder(_layouter, new TagCloudSettings());
    }

    [Test]
    public void BuildTagCloud_ShouldReturnCorrectCount_WhenValidParameters()
    {
        var result = _builder.GetTagCloud(_frequency, 3);

        result.Should().HaveCount(3);
        result[0].word.Should().Be("гарри");
        result[1].word.Should().Be("поттер");
        result[2].word.Should().Be("хогвартс");
    }

    [Test]
    public void BuildTagCloud_ShouldThrowArgumentException_WhenMaxFontSizeLessThanMin()
    {
        var act = () => _builder.GetTagCloud(_frequency, 3, 20, 10);

        act.Should().Throw<ArgumentException>()
            .WithMessage("maxFontSize must be greater than minFontSize");
    }

    [Test]
    public void BuildTagCloud_ShouldThrowArgumentException_WhenCountLessZero()
    {
        var act = () => _builder.GetTagCloud(_frequency, -1, 10, 100);

        act.Should().Throw<ArgumentException>()
            .WithMessage("tagCount must be greater than or equal to zero");
    }
    
    [Test]
    public void BuildTagCloud_ShouldUseAllWords_WhenTagCountNotSpecified()
    {
        var result = _builder.GetTagCloud(_frequency);

        result.Should().HaveCount(_frequency.Count);
    }

    [Test]
    public void BuildTagCloud_ShouldHaveLargerRectanglesForMoreFrequentWords_WhenDifferentFrequencies()
    {
        var result = _builder.GetTagCloud(_frequency, 3, 10, 100);

        var size1 = result[0].rect.Size;
        var size2 = result[1].rect.Size;

        size1.Width.Should().BeGreaterThan(size2.Width);
        size1.Height.Should().BeGreaterThan(size2.Height);
    }


    [Test]
    public void BuildTagCloud_ShouldEqualFontSize_WhenAllFrequenciesAreEqual()
    {
        var sameFreq = new Dictionary<string, int> { ["слово1"] = 100, ["слово2"] = 100 };

        var result = _builder.GetTagCloud(sameFreq, 2, 10, 500);

        result[0].rect.Width.Should().Be(result[1].rect.Width);
        result[0].rect.Height.Should().Be(result[1].rect.Height);
    }

    [Test]
    public void BuildTagCloud_ShouldHaveLargerWidthForLongerWords_WhenSameFrequency()
    {
        var sameFreq = new Dictionary<string, int> { ["короткое"] = 100, ["очень_длинное_слово"] = 100 };

        var result = _builder.GetTagCloud(sameFreq, 2, 10, 500);

        var shortWidth = result[0].rect.Width;
        var longWidth = result[1].rect.Width;

        longWidth.Should().BeGreaterThan(shortWidth);
        result[0].rect.Height.Should().Be(result[1].rect.Height);
    }
}