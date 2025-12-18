namespace TagsCloudContainer.Lemmatization;

public static class PartsOfSpeech
{
    public const string Noun = "S";
    public const string Adjective = "A";
    public const string Verb = "V";
    public const string Adverb = "ADV";
    public const string Conjunction = "CONJ";
    public const string Pronoun = "PR";

    public static readonly HashSet<string> ContentWords =
        new(StringComparer.OrdinalIgnoreCase)
        {
            Noun, Adjective, Verb
        };
}