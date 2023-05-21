
namespace nWeaveTask.BL.DTOs.User;

public record RegisterDTO
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}
