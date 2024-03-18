using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Benchmarking.Linq;
using Microsoft.Extensions.DependencyInjection;

// TASKT: Logging

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ParquetReaderServiceOptions>(
    builder.Configuration.GetSection(nameof(ParquetReaderServiceOptions)));

ParquetReaderServiceOptions options = new("data");

using IHost host = builder.Build();
await host.RunAsync();
