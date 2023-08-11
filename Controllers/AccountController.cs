using ECommerceWebApplication.Data;
using ECommerceWebApplication.Data.Static;
using ECommerceWebApplication.Data.ViewModels;
using ECommerceWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebApplication.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Users()
    {
        var users =
            await _context.Users.ToListAsync().ConfigureAwait(true);
        return View(users);
    }

    public IActionResult Login()
    {
        return View(new LoginVM());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress).ConfigureAwait(true);
        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password).ConfigureAwait(true);
            if (passwordCheck)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(user, loginVM.Password, false, false).ConfigureAwait(true);
                if (result.Succeeded) return RedirectToAction("Index", "Movies");
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        TempData["Error"] = "Wrong credentials. Please, try again!";
        return View(loginVM);
    }

    public IActionResult Register()
    {
        return View(new RegisterVM());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid) return View(registerVM);

        var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress).ConfigureAwait(true);
        if (user != null)
        {
            TempData["Error"] = "This email address is already in use";
            return View(registerVM);
        }

        ApplicationUser newUser = new()
        {
            FullName = registerVM.FullName,
            Email = registerVM.EmailAddress,
            UserName = registerVM.EmailAddress
        };
        var newUserResponse =
            await _userManager.CreateAsync(newUser, registerVM.Password).ConfigureAwait(true);

        if (newUserResponse.Succeeded)
            _ = await _userManager.AddToRoleAsync(newUser, UserRoles.User).ConfigureAwait(true);

        return View("RegisterCompleted");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(true);
        return RedirectToAction("Index", "Movies");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}