using System.Reflection;
using Microsoft.Extensions.Options;
using Parquet;
using Parquet.Data;
using Parquet.Serialization;

namespace Benchmarking.Linq;

public class ParquetReaderService(string filePath, IOptionsSnapshot<ParquetReaderServiceOptions> options)
{
    private readonly ParquetReaderServiceOptions serviceOptions = options.Value;
    
    /// <summary>
    /// Reads all rows in a Parquet file.
    /// </summary>
    /// <typeparam name="T">Type to deserialize each row into.</typeparam>
    /// <param name="filePath">Path to a Parquet file to read from. Absolute or relative to the <see cref="ParquetReaderServiceOptions.ParquetDataPath"/> path.</param>
    /// <returns>A collection of <typeparamref name="T"/> type objects representing deserialized rows.</returns>
    public async Task<IEnumerable<T>> ReadFileRows<T>(string filePath) where T : new()
    {
        var ret = new List<T>();
        using var reader = await OpenReader(filePath);
        if (reader.RowGroupCount == 0)
            return ret;
        for (var rowGroupIndex = 0; rowGroupIndex < reader.RowGroupCount; rowGroupIndex++)
        {
            // TASKT: Separate path and filename.
            var rgData = await ParquetSerializer.DeserializeAsync<T>(serviceOptions.ParquetDataPath, rowGroupIndex);
            ret.AddRange(rgData);
        }

        return ret.AsEnumerable();
    }

    public async Task GenerateSchemaClass(string parquetPath, string outputPath)
    {
        using var reader = await OpenReader(parquetPath);
        var schema = reader.Schema;
        var generator = new ClassGenerator(schema);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private FileStream OpenStream(string filePath)
    {
        // TASKT: Validation etc.
        return File.OpenRead(filePath);
    }    
    
    private async Task<ParquetReader> OpenReader(string filePath)
    {
        var reader = await ParquetReader.CreateAsync(OpenStream(Path.Combine(serviceOptions.ParquetDataPath, filePath)), serviceOptions.ParquetReaderOptions);
        return reader;
    }
}