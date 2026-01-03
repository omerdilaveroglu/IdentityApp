using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels;

public class EditViewModel
{
    public String id { get; set; } = String.Empty;

    public String FullName { get; set; } = String.Empty;

    [EmailAddress]
    public string? Email { get; set; } = String.Empty;


    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; } = String.Empty;

}
