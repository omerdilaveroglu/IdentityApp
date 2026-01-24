using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels;

public class CreateViewModel
{

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public String FullName { get; set; } = String.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = String.Empty;

}
