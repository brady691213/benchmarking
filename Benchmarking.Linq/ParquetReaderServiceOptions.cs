using System.Runtime.CompilerServices;
using Parquet;

namespace Benchmarking.Linq;

public class ParquetReaderServiceOptions(string dataPath, bool bytesAsStrings = true)
{
    public ParquetOptions ParquetReaderOptions { get; init; } = new() { TreatByteArrayAsString = bytesAsStrings };

    public string DataPath { get; init; } = dataPath;
}