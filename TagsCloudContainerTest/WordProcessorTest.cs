using FluentAssertions;
using TagsCloudContainer.WordProcess;

namespace TagsCloudContainerTest;

public class WordProcessorTest
{
    private WordProcessor wordProcessor;
    private List<string> words;

    [SetUp]
    public void Setup()
    {
        wordProcessor = new WordProcessor();

        words =
        [
            "красивые", "маме", "в", "под", "рамой", "мыла", "и", "он",
        ];
    }

    [Test]
    public void ProcessWords_ShouldReturnLemmasInBaseForm_WhenGivenMixedInput()
    {
        var result = wordProcessor.Get(words);

        result.Should().Contain("красивый");
        result.Should().Contain("мама");
        result.Should().Contain("рама");


        result.Should().NotContain("красивые");
        result.Should().NotContain("маме");
        result.Should().NotContain("рамой");
    }

    [Test]
    public void ProcessWords_ShouldExcludeSpecificWords_WhenConfigured()
    {
        var result = wordProcessor
            .SetAllowedPartsOfSpeech(["S"])
            .ExcludeWords(["рама"])
            .Get(words);

        result.Should().Contain("мама");
        result.Should().NotContain("рама");
        result.Should().NotContain("раму");
        result.Should().NotContain("красивый");
        result.Should().NotContain("красивые");
    }
}