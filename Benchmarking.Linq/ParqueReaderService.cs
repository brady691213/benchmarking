using System.Reflection;
using Parquet;
using Parquet.Serialization;

namespace Benchmarking.Linq;

// TASKQ How to get a better default value for ParquetReaderOptions?
public class ParquetReaderService(string filePath, ParquetReaderServiceOptions? options = null)
{
    private FileStream OpenStream(string filePath)
    {
        // TASKT: Validation etc.
        return File.OpenRead(filePath);
    }

    /// <summary>
    /// Reads all rows in a Parquet file.
    /// </summary>
    /// <typeparam name="T">Type to deserialize each row into.</typeparam>
    /// <param name="filePath">Path to a Parquet file to read from. Absolute or relative to the <see cref="ParquetReaderServiceOptions.ParquetDataPath"/> path.</param>
    /// <returns>A collection of <typeparamref name="T"/> type objects representing deserialized rows.</returns>
    public async Task<IEnumerable<T>> ReadFileRows<T>(string filePath) where T : new()
    {
        var ret = new List<T>();
        var reader = await ParquetReader.CreateAsync(OpenStream(Path.Combine(options?.ParquetDataPath ?? "data", filePath)), options?.ParquetReaderOptions);
        if (reader.RowGroupCount == 0)
            return ret;
        for (var rowGroupIndex = 0; rowGroupIndex < reader.RowGroupCount; rowGroupIndex++)
        {
            // TASKT: Separate path and filename.
            var rgData = await ParquetSerializer.DeserializeAsync<T>(options.ParquetDataPath, rowGroupIndex);
            ret.AddRange(rgData);
        }   
        return ret;
    }
}