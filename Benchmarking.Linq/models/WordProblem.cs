using Parquet.Serialization.Attributes;

namespace Benchmarking.Linq.models;

public record WordProblem
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public WordProblem(string question, string answer)
    {
        this.Question = question;
        this.Answer = answer;
    }

    [ParquetRequired]
    public string Question { get; init; }
    
    [ParquetRequired]
    public string Answer { get; init; }

    public void Deconstruct(out string question, out string answer)
    {
        question = this.Question;
        answer = this.Answer;
    }
}