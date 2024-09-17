using FluentResults;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHybridCache();

IHost host = builder.Build();

var cache = host.Services.GetRequiredService<HybridCache>();

Result<List<string>> cachedResults = await cache.GetOrCreateAsync(
    $"CachedResults",
    async (cancel) => await GetResults(),
    cancellationToken: CancellationToken.None
);

Console.WriteLine(cachedResults.IsSuccess); // Result is successful
Console.WriteLine(cachedResults.Value); // Value is null

Task<Result<List<string>>> GetResults() 
{
    var results = Result.Ok<List<string>>(["Result 1", "Result 2", "Result 3"]);
    
    Console.WriteLine(results.IsSuccess); // Result is successful
    Console.WriteLine(results.Value); // Value is not null

    return Task.FromResult(results);
}

await host.RunAsync();
