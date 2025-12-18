using CommandLine;

namespace TagsCloudContainer.Client;

[Verb("generate", HelpText = "Generate tag cloud from text file")]
public class GenerateOptions
{
    [Option('i', "input", Required = false, 
        Default = "SourceFile/ГарриПоттерНаРусском.txt",
        HelpText = "SourceFile/ГарриПоттерНаРусском.txt)")]
    public string InputFilePath { get; set; } = "SourceFile/ГарриПоттерНаРусском.txt";

    [Option('o', "output", Required = false, Default = "tagCloud.png", HelpText = "Output image path")]
    public string OutputFilePath { get; set; } = "";

    [Option('t', "tags", Default = 150, HelpText = "Tags count")]
    public int TagCount { get; set; } = 150;

    [Option('m', "min-font", Default = 10, HelpText = "Min font size")]
    public int MinFontSize { get; set; } = 10;

    [Option('M', "max-font", Default = 90, HelpText = "Max font size")]
    public int MaxFontSize { get; set; } = 90;

    [Option('e', "exclude", Separator = ',', HelpText = "Words to exclude")]
    public IEnumerable<string> ExcludeWords { get; set; } = [];

    [Option('l', "min-length", Default = 2, HelpText = "Min word length")]
    public int MinWordLength { get; set; } = 2;
}