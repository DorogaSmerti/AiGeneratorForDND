using System.Text.Json.Nodes;
using StoryTracker.Models;

namespace StoryTracker.Service.Interface;

public interface IVectorService
{
    /// <summary>
    /// Performs a semantic vector search over the item compendium to find the best matching item.
    /// </summary>
    /// <param name="query">Item description or name to search for.</param>
    /// <returns>The most relevant item JsonNode, or failure if no match is found.</returns>
    Task<Result<JsonNode>> SearchAsync(string query, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generates and updates vector embeddings for all unindexed items in the local compendium database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to interrupt the indexing process.</param>
    /// <returns>The total number of vector embeddings stored in the cache database upon completion.</returns>
    Task<Result<int>> BuildVectorDataBaseAsync(CancellationToken cancellationToken = default);
}