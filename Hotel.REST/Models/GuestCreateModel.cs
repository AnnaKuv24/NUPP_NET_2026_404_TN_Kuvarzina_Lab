using System.ComponentModel.DataAnnotations;

namespace Hotel.REST.Models;

public class GuestCreateModel
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}