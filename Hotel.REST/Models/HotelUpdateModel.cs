using System.ComponentModel.DataAnnotations;

namespace Hotel.REST.Models;

public class HotelUpdateModel
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string City { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Stars { get; set; }
}