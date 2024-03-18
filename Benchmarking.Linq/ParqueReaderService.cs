using Parquet;

namespace Benchmarking.Linq;

public class ParquetReaderService(ParquetReaderServiceOptions options)
{
    private FileStream OpenStream(string filePath)
    {
        // TODO: Validation etc.
        return File.OpenRead(filePath);
    }

    public async Task<IEnumerable<T>> ReadAllRows<T>()
    {
        var ret = new List<T>();
        var reader = await ParquetReader.CreateAsync(OpenStream(options.DataPath), options.ParquetReaderOptions);
        if (reader.RowGroupCount == 0)
            return ret;
        for (int rowGroup = 0; rowGroup < reader.RowGroupCount; rowGroup++)
        {
            var group = reader.OpenRowGroupReader(rowGroup);            
            var rows = await group.g
            ret.AddRange(rows);
        }
    }
}