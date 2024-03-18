using System.Reflection;
using System.Runtime.CompilerServices;
using Parquet;

namespace Benchmarking.Linq;

public class ParquetReaderServiceOptions(string? dataPath = null, bool bytesAsStrings = true)
{
    public ParquetOptions ParquetReaderOptions { get; init; } = new() { TreatByteArrayAsString = bytesAsStrings };

    public string ParquetDataPath { get; init; } = string.IsNullOrWhiteSpace(dataPath) ? Path.Combine(Assembly.GetExecutingAssembly().Location, "data") : dataPath;
}