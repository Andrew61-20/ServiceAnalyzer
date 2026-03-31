using ServiceAnalyzer.Domain.Entities;
using System;
using System.Text.Json;

namespace ServiceAnalyzer.Infrastructure.Clients
{
    public class RedditClient
    {
        private readonly HttpClient _httpClient;

        public RedditClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RedditPost>> GetPostsAsync(string subreddit, int limit)
        {
            try
            {
                var normalized = subreddit.StartsWith("/r/")
                    ? subreddit
                    : $"/r/{subreddit.Replace("r/", "")}";
                var url = $"{normalized}/top.json?limit={limit}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Reddit API error: {response.StatusCode}");

                var content = await response.Content.ReadAsStringAsync();

                using var json = JsonDocument.Parse(content);

                var posts = new List<RedditPost>();

                foreach (var post in json.RootElement
                                .GetProperty("data")
                                .GetProperty("children")
                                .EnumerateArray())
                {
                    var data = post.GetProperty("data");

                    posts.Add(new RedditPost
                    {
                        Title = data.GetProperty("title").GetString(),
                        SelfText = data.GetProperty("selftext").GetString(),
                        HasImage = data.TryGetProperty("post_hint", out var hint) &&
                            hint.GetString() == "image"
                    });
                }

                return posts;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Network error: " + ex.Message);
            }
        }
    }
}
