using TagsCloudContainer.TextReader;
using FluentAssertions;

namespace TagsCloudContainerTest;

public class TagsTextReaderTest
{
    private TextFileProcessor _textFileProcessor;
    
    [SetUp]
    public void Setup()
    {
        _textFileProcessor = new TextFileProcessor();
    }

    [Test]
    public void GetWordsFromFile_ShouldReadAllWords_WhenTxtFileProvided()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "txtTest.txt");

        var words = _textFileProcessor.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void GetWordsFromFile_ShouldReadAllWords_WhenDocxFileProvided()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "docxTest.docx");

        var words = _textFileProcessor.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void GetWordsFromFile_ShouldReadAllWords_WhenDocFileProvided()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "docTest.doc");

        var words = _textFileProcessor.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void GetWordsFromFile_ShouldReadAllWords_WhenLargeEnglishTextProvided()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "HarryPotterText.txt");

        var words = _textFileProcessor.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(82516);
    }
    
    [Test]
    public void GetWordsFromFile_ShouldReadAllWords_WhenLargeRussianTextProvided()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "Гарри Поттер и философский камень.txt");

        var words = _textFileProcessor.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(63922);
    }
}