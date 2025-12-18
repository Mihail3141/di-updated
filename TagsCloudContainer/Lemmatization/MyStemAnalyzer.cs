using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;


namespace TagsCloudContainer.Lemmatization;

public class MyStemAnalyzer : IStemAnalyzer
{
    private readonly string _systemPath = Path.Combine(AppContext.BaseDirectory, "mystem.exe");
    private readonly ConcurrentDictionary<string, List<MyStemWord>> _cache = new();
    

    public List<MyStemWord> AnalyzeBatch(IEnumerable<string> words)
    {
        var uniqueWords = words.Distinct(StringComparer.OrdinalIgnoreCase).ToHashSet();

        var cached = new List<MyStemWord>();
        var toProcess = new List<string>();

        foreach (var word in uniqueWords)
        {
            if (_cache.TryGetValue(word, out var cachedAnalysis) && cachedAnalysis.Count != 0)
                cached.AddRange(cachedAnalysis);
            else
                toProcess.Add(word);
        }

        var newAnalysis = ProcessBatch(toProcess);

        foreach (var word in toProcess)
        {
            var wordAnalysis = newAnalysis.Where(w => w.Text.Equals(word, StringComparison.OrdinalIgnoreCase)).ToList();
            _cache[word] = wordAnalysis;
        }

        cached.AddRange(newAnalysis);
        return cached;
    }

    private List<MyStemWord> ProcessBatch(List<string> words)
    {
        var inputText = string.Join("\n", words);

        using var process = StartMyStemProcess();
        WriteInput(process, inputText);
        var output = ReadOutput(process, out _, out _);

        return ParseOutput(output);
    }

    private Process StartMyStemProcess()
    {
        var psi = new ProcessStartInfo
        {
            FileName = _systemPath,
            Arguments = "-n -i --format json",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            StandardInputEncoding = Encoding.UTF8,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = psi };
        process.Start();
        return process;
    }

    private static void WriteInput(Process process, string input)
    {
        using var writer = process.StandardInput;
        writer.WriteLine(input);
        writer.Close();
    }

    private static string ReadOutput(Process process, out string stderr, out int exitCode)
    {
        var output = process.StandardOutput.ReadToEnd();
        stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        exitCode = process.ExitCode;
        return output;
    }

    private static List<MyStemWord> ParseOutput(string output)
    {
        var result = new List<MyStemWord>();

        var lines = output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var item = JsonSerializer.Deserialize<JsonElement>(line.Trim());

            if (!item.TryGetProperty("analysis", out var analysis) || analysis.GetArrayLength() == 0)
                continue;

            var chosenAnalysis = analysis[0];

            if (!chosenAnalysis.TryGetProperty("lex", out var lex))
                continue;

            var gr = chosenAnalysis.TryGetProperty("gr", out var grProp) ? grProp.GetString() ?? "" : "";

            result.Add(new MyStemWord
            {
                Text = item.TryGetProperty("text", out var textProp) ? textProp.GetString() ?? "" : "",
                Lemma = lex.GetString() ?? "",
                Pos = ParsePosTag(gr)
            });
        }

        return result;
    }


    private static string ParsePosTag(string gr)
    {
        var parts = gr.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0].Trim().Split('=')[0].Trim() : string.Empty;
    }
}