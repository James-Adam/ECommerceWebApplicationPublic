using ECommerceWebApplication.Data.Services;
using ECommerceWebApplication.Data.Static;
using ECommerceWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApplication.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ActorsController : Controller
{
    private readonly IActorsService _service;

    public ActorsController(IActorsService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync().ConfigureAwait(true);
        return View(data);
    }

    //Get: Actors/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Actor actor)
    {
        if (!ModelState.IsValid) return View(actor);

        await _service.AddAsync(actor).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }

    //Get: Actors/Details/1
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var actorDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);

        return actorDetails == null ? View("NotFound") : (IActionResult)View(actorDetails);
    }

    //Get: Actors/Edit/1
    public async Task<IActionResult> Edit(int id)
    {
        var actorDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return actorDetails == null ? View("NotFound") : (IActionResult)View(actorDetails);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Actor actor)
    {
        if (!ModelState.IsValid) return View(actor);

        await _service.UpdateAsync(id, actor).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }

    //Get: Actors/Delete/1
    public async Task<IActionResult> Delete(int id)
    {
        var actorDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return actorDetails == null ? View("NotFound") : (IActionResult)View(actorDetails);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var actorDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        if (actorDetails == null) return View("NotFound");

        await _service.DeleteAsync(id).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }
}