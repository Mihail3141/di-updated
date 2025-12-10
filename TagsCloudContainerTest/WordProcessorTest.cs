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
        var mystemPath = Path.Combine(AppContext.BaseDirectory, "mystem.exe");

        wordProcessor = new WordProcessor(mystemPath);

        words =
        [
            "красивые", "маме", "в", "под", "рамой", "мыла", "и", "он", 
        ];
    }

    [Test]
    public void ВсеСловаКНачальнойФорме()
    {
        var result = wordProcessor.ProcessWords(words);

        result.Should().Contain("красивый");
        result.Should().Contain("мама");
        result.Should().Contain("рама");
  
        
        result.Should().NotContain("в");
        result.Should().NotContain("под");
        result.Should().NotContain("и");
        result.Should().NotContain("он");
        
    }
}