namespace TagsCloudContainer.Lemmatization;

public interface IStemAnalyzer
{
    public List<MyStemWord> AnalyzeBatch(IEnumerable<string> words);
}