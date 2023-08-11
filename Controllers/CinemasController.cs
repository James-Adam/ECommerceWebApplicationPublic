using ECommerceWebApplication.Data.Services;
using ECommerceWebApplication.Data.Static;
using ECommerceWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApplication.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class CinemasController : Controller
{
    private readonly ICinemasService _service;

    public CinemasController(ICinemasService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var allCinemas =
            await _service.GetAllAsync().ConfigureAwait(true);
        return View(allCinemas);
    }

    //Get: Cinemas/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Cinema cinema)
    {
        if (!ModelState.IsValid) return View(cinema);

        await _service.AddAsync(cinema).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }

    //Get: Cinemas/Details/1
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var cinemaDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return cinemaDetails == null ? View("NotFound") : (IActionResult)View(cinemaDetails);
    }

    //Get: Cinemas/Edit/1
    public async Task<IActionResult> Edit(int id)
    {
        var cinemaDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return cinemaDetails == null ? View("NotFound") : (IActionResult)View(cinemaDetails);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
    {
        if (!ModelState.IsValid) return View(cinema);

        await _service.UpdateAsync(id, cinema).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }

    //Get: Cinemas/Delete/1
    public async Task<IActionResult> Delete(int id)
    {
        var cinemaDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return cinemaDetails == null ? View("NotFound") : (IActionResult)View(cinemaDetails);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var cinemaDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        if (cinemaDetails == null) return View("NotFound");

        await _service.DeleteAsync(id).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }
}