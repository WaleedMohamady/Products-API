

using System.ComponentModel.DataAnnotations;

namespace nWeaveTask.BL.DTOs.User;

public class LoginDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

}
