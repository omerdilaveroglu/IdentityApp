using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class RolesController : Controller
    {
        // GET: RolesController

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]        
        public IActionResult Create(AppRole model)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole
                {
                    Name = model.Name
                };
                var result =  _roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }


        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }



        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);  
                return View(role);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

    }
}
