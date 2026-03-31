using ServiceAnalyzer.Application.DTO;
using ServiceAnalyzer.Application.Interfaces;
using ServiceAnalyzer.Infrastructure.Clients;
using ServiceAnalyzer.Logging;
using System;

namespace ServiceAnalyzer.Application.Services
{
    public class RedditService : IRedditService
    {
        private readonly RedditClient _client;
        private readonly ILogger<RedditService> _logger;

        public RedditService(RedditClient client, ILogger<RedditService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<AnalyzeResponseDto> AnalyzeAsync(AnalyzeRequestDto request)
        {
            _logger.LogInformation("Start analyze");

            var result = new AnalyzeResponseDto();

            var tasks = request.Items.Select(async item =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item.Subreddit))
                        throw new ArgumentException("Subreddit is required");

                    _logger.LogInformation($"Processing {item.Subreddit}");

                    var posts = await _client.GetPostsAsync(item.Subreddit, request.Limit);

                    _logger.LogInformation($"Posts count: {posts.Count}");

                    var filtered = posts
                        .Where(p =>
                            item.Keywords == null || !item.Keywords.Any() ||
                            item.Keywords.Any(k =>
                                (p.Title?.Contains(k, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (p.SelfText?.Contains(k, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                        )
                        .Select(p => new PostResultDto
                        {
                            Title = p.Title,
                            HasImage = p.HasImage
                        })
                        .ToList();
 
                    _logger.LogInformation($"Found {filtered.Count} posts in {item.Subreddit}");

                    return new { item.Subreddit, Filtered = filtered };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in {item.Subreddit}");
                    return new { item.Subreddit, Filtered = new List<PostResultDto>() };
                }
            });

            var results = await Task.WhenAll(tasks);

            foreach (var r in results)
            {
                result.Data[r.Subreddit] = r.Filtered;
            }

            _logger.LogInformation("Analyze finished");

            return result;
        }
    }
}
