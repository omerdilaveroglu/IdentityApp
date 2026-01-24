using System;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityApp.TagHelpers;

[HtmlTargetElement("td", Attributes = "asp-role-users")]
public class RoleUsersTagHelpers: TagHelper
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    public RoleUsersTagHelpers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [HtmlAttributeName("asp-role-users")]
    public string Role { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var userNames = new List<string>();
        var role = await _roleManager.FindByIdAsync(Role);
     
        if (role != null)
        {
            var users = _userManager.Users;

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    userNames.Add(user.UserName ?? "");
                }
            }
        }

        output.Content.SetHtmlContent(userNames.Count == 0 ? "No users" : setHtml(userNames));
        
    }


    private string setHtml(List<string> userNames)
    {
        var html ="<ul>";
        foreach (var userName in userNames)
        {
            html += $"<li>{userName}</li>";
        }
        html += "</ul>";
        return html;
    }
}
