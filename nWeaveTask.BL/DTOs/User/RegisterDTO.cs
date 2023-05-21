
using System.ComponentModel.DataAnnotations;

namespace nWeaveTask.BL.DTOs.User;

public record RegisterDTO
{
    [Required]
    public string FirstName { get; init; }
    [Required]
    public string LastName { get; init; }
    [Required]
    public string Email { get; init; }
    [Required]
    public string Password { get; init; }
}
