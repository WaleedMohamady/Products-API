
namespace nWeaveTask.BL.DTOs.Token;

public record TokenDTO
{
    public string Token { get; init; }
    public DateTime ExpiryDate { get; init; }
}
