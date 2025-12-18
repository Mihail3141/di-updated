namespace TagsCloudContainer.Lemmatization;

public class WordProcessor
{
    private readonly IStemAnalyzer _analyzer;
    private HashSet<string> _allowedPartsOfSpeech = PartsOfSpeech.ContentWords;
    private HashSet<string> _excludedWords = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Func<MyStemWord, bool>> _filters = [];

    public WordProcessor(IStemAnalyzer analyzer)
    {
        _analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
    }

    public WordProcessor SetAllowedPartsOfSpeech(IEnumerable<string> allowedPartsOfSpeech)
    {
        _allowedPartsOfSpeech = allowedPartsOfSpeech.ToHashSet();
        return this;
    }

    public WordProcessor ExcludeWords(IEnumerable<string> excludedWords)
    {
        _excludedWords = excludedWords.ToHashSet();
        return this;
    }

    public WordProcessor AddWordFilter(Func<MyStemWord, bool> filter)
    {
        _filters.Add(filter ?? throw new ArgumentNullException(nameof(filter)));
        return this;
    }

    public List<string> Get(IEnumerable<string> words)
    {
        var batches = words
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Chunk(800)
            .ToList();

        var allAnalysis = new List<MyStemWord>();

        Parallel.ForEach(batches, batch =>
        {
            var batchAnalysis = _analyzer.AnalyzeBatch(batch);
            lock (allAnalysis)
            {
                allAnalysis.AddRange(batchAnalysis);
            }
        });

        return allAnalysis
            .Where(w => _allowedPartsOfSpeech.Contains(w.Pos))
            .Where(word => !_excludedWords.Contains(word.Lemma.ToLowerInvariant()))
            .Where(word => _filters.Count == 0 || _filters.All(filter => filter(word)))
            .Select(w => w.Lemma.ToLowerInvariant())
            .ToList();
    }
}