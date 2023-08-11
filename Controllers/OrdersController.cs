using System.Security.Claims;
using ECommerceWebApplication.Data.Cart;
using ECommerceWebApplication.Data.Services;
using ECommerceWebApplication.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApplication.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IMoviesService _moviesService;
    private readonly IOrdersService _ordersService;
    private readonly ShoppingCart _shoppingCart;

    public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart, IOrdersService ordersService)
    {
        _moviesService = moviesService;
        _shoppingCart = shoppingCart;
        _ordersService = ordersService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var orders =
            await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole).ConfigureAwait(true);
        return View(orders);
    }

    public IActionResult ShoppingCart()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        _shoppingCart.ShoppingCartItems = items;

        ShoppingCartVM response = new()
        {
            ShoppingCart = _shoppingCart,
            ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
        };

        return View(response);
    }

    public async Task<IActionResult> AddItemToShoppingCart(int id)
    {
        var item = await _moviesService.GetMovieByIdAsync(id).ConfigureAwait(true);

        if (item != null) _shoppingCart.AddItemToCart(item);

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
    {
        var item = await _moviesService.GetMovieByIdAsync(id).ConfigureAwait(true);

        if (item != null) _shoppingCart.RemoveItemFromCart(item);

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> CompleteOrder()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

        await _ordersService.StoreOrderAsync(items, userId, userEmailAddress).ConfigureAwait(true);
        await _shoppingCart.ClearShoppingCartAsync().ConfigureAwait(true);

        return View("OrderCompleted");
    }
}