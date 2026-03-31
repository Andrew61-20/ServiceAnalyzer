namespace ServiceAnalyzer.Application.DTO
{
    public class AnalyzeRequestDto
    {
        public List<SubredditItemDto> Items { get; set; } = new();
        public int Limit { get; set; }
    }
    public class SubredditItemDto
    {
        public string Subreddit { get; set; } = string.Empty;
        public List<string>? Keywords { get; set; }
    }
}
