using TagsCloudContainer.TextReader;
using FluentAssertions;

namespace TagsCloudContainerTest;

public class TagsCloudContainerTest
{
    private Reader _reader;
    
    [SetUp]
    public void Setup()
    {
        _reader = new Reader();
    }

    [Test]
    public void TxtTest()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "txtTest.txt");

        var words = _reader.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void DocxTest()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "docxTest.docx");

        var words = _reader.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void DocTest()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "docTest.doc");

        var words = _reader.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(19);
    }
    
    [Test]
    public void HarryPotterTest()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "HarryPotterText.txt");

        var words = _reader.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(82516);
    }
    
    [Test]
    public void RuHarryPotterTest()
    {
        var filePath = Path.Combine("..", "..", "..", "TestFiles", "Гарри Поттер и философский камень.txt");

        var words = _reader.GetWordsFromFile(filePath);
        
        words.Should().HaveCount(63922);
    }
}