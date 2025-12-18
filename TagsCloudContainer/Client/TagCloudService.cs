using System.Drawing;
using TagsCloudContainer.Lemmatization;
using TagsCloudContainer.TagBuilder;
using TagsCloudContainer.TagsCloudVisualization.Render;
using TagsCloudContainer.TextReader;

namespace TagsCloudContainer.Client;

public class TagCloudService(
    TextFileProcessor textFileProcessor,
    WordProcessor wordProcessor,
    ITagCloudBuilder builder,
    ICloudRenderer renderer) : ITagCloudService
{
    public Dictionary<string, int> GetWordFrequency(
        string inputFilePath,
        string[] allowedPartsOfSpeech,
        string[] excludeWords,
        int minWordLength = 2)
    {
        var wordsFromText = textFileProcessor.GetWordsFromFile(inputFilePath);

        return wordProcessor
            .SetAllowedPartsOfSpeech(allowedPartsOfSpeech)
            .ExcludeWords(excludeWords)
            .AddWordFilter(word => word.Lemma.Length > minWordLength)
            .Get(wordsFromText)
            .GroupBy(word => word)
            .ToDictionary(group => group.Key, group => group.Count());
    }
    
    public void GenerateTagCloud(string inputFile,
        string outputPath,
        TagCloudSettings options)       
    {
        var words = textFileProcessor.GetWordsFromFile(inputFile);
        
        var processedWords = wordProcessor
            .ExcludeWords(options.ExcludeWords)  
            .AddWordFilter(w => w.Lemma.Length > options.MinWordLength)
            .Get(words);

        var frequency = processedWords
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count())
            .OrderByDescending(x => x.Value)
            .Take(options.TagsCount)
            .ToDictionary(x => x.Key, x => x.Value);

        var tags = builder.GetTagCloud(frequency, options.TagsCount, options.MinFontSize, options.MaxFontSize);
        var bitmap = renderer.CreateRectangleCloud(tags);
        
        bitmap.Save(outputPath);  
    }
}