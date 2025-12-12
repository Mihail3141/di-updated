using System.Collections.Concurrent;
using TagsCloudContainer.WordProcess;

namespace TagsCloudContainer.WordProcess;

public class WordProcessor
{
    private readonly MyStemAnalyzer _analyzer;
    private readonly ConcurrentDictionary<string, List<MyStemWord>> _cache = new();


    private HashSet<string> _borningPos = new(StringComparer.OrdinalIgnoreCase)
    {
        "PR", "CONJ", "CCONJ", "INTJ", "P", "ADV-PRON", "PRED", "PART", "A-PRON",
    };

    private HashSet<string> _allowedPos = new(StringComparer.OrdinalIgnoreCase) { "S", "A",  };

    public WordProcessor(string systemPath)
    {
        _analyzer = new MyStemAnalyzer(systemPath);
    }

    public List<string> ProcessWords(IEnumerable<string> words)
    {
        var batches = words
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Chunk(100)
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
            .Where(w => _allowedPos.Contains(w.Pos))
            .Select(w => w.Lemma.ToLowerInvariant())
            .ToList();
    }
}