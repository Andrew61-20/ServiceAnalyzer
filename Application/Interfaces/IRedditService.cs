using ServiceAnalyzer.Application.DTO;

namespace ServiceAnalyzer.Application.Interfaces
{
    public interface IRedditService
    {
        Task<AnalyzeResponseDto> AnalyzeAsync(AnalyzeRequestDto request);
    }
}
