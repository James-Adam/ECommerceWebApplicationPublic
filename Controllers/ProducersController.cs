using ECommerceWebApplication.Data.Services;
using ECommerceWebApplication.Data.Static;
using ECommerceWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApplication.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ProducersController : Controller
{
    private readonly IProducersService _service;

    public ProducersController(IProducersService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var allProducers =
            await _service.GetAllAsync().ConfigureAwait(true);
        return View(allProducers);
    }

    //GET: producers/details/1
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var producerDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return producerDetails == null ? View("NotFound") : (IActionResult)View(producerDetails);
    }

    //GET: producers/create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Producer producer)
    {
        if (!ModelState.IsValid) return View(producer);

        await _service.AddAsync(producer).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }

    //GET: producers/edit/1
    public async Task<IActionResult> Edit(int id)
    {
        var producerDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return producerDetails == null ? View("NotFound") : (IActionResult)View(producerDetails);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Producer producer)
    {
        if (!ModelState.IsValid) return View(producer);

        if (id == producer.Id)
        {
            await _service.UpdateAsync(id, producer).ConfigureAwait(true);
            return RedirectToAction(nameof(Index));
        }

        return View(producer);
    }

    //GET: producers/delete/1
    public async Task<IActionResult> Delete(int id)
    {
        var producerDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        return producerDetails == null ? View("NotFound") : (IActionResult)View(producerDetails);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producerDetails = await _service.GetByIdAsync(id).ConfigureAwait(true);
        if (producerDetails == null) return View("NotFound");

        await _service.DeleteAsync(id).ConfigureAwait(true);
        return RedirectToAction(nameof(Index));
    }
}