using System.ComponentModel.DataAnnotations;

namespace Word.API.Contracts.Auth;

public class ChangeLanguageRequest
{
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string LanguageCode { get; set; } = string.Empty; // ky или ru
}