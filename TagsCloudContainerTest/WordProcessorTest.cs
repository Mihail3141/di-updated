using FluentAssertions;
using TagsCloudContainer.Lemmatization;

namespace TagsCloudContainerTest;

public class LemmatizationTest
{
    private WordProcessor wordProcessor;
    private List<string> words;

    [SetUp]
    public void Setup()
    {
        wordProcessor = new WordProcessor(new MyStemAnalyzer());

        words = ["красивые", "маме", "в", "под", "рамой", "мыла", "и", "он",];
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
            .AddWordFilter(word => word.Lemma[0] == 'м')
            .Get(words);

        result.Should().Contain("мама");
        result.Should().Contain("мыло");
        result.Should().NotContain("рама");
        result.Should().NotContain("красивый");
    }
    
    [Test]
    public void ProcessWords_ShouldExcludeSpecificWordsФ_WhenConfigured()
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