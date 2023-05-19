

namespace nWeaveTask.BL.DTOs;

public class ProductUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageURL { get; set; } = "";
}
