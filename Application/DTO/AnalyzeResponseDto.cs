namespace ServiceAnalyzer.Application.DTO
{
    public class PostResultDto
    {
        public string? Title { get; set; }
        public bool HasImage { get; set; }
    }
    public class AnalyzeResponseDto
    {
        public Dictionary<string, List<PostResultDto>> Data { get; set; } = new();
    }
}
