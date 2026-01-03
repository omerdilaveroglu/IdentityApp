using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class UsersController : Controller
    {
        // GET: UsersController

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        private readonly UserManager<AppUser> _userManager;
        public ActionResult Index()
        {
            return View(_userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModels.CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(new ViewModels.EditViewModel
                {
                    id = user.Id,
                    FullName = user.FullName ?? String.Empty,
                    Email = user.Email
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ViewModels.EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // EĞER ŞİFRE ALANI DOLUYSA ŞİFREYİ GÜNCELLE
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            // Önce mevcut şifreyi kaldır
                            await _userManager.RemovePasswordAsync(user);
                            // Yeni şifreyi ekle
                            var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);

                            if (!passwordResult.Succeeded)
                            {
                                foreach (var error in passwordResult.Errors)
                                    ModelState.AddModelError("", error.Description);
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }

    }

}
