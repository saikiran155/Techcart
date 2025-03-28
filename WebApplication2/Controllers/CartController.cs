using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techcart.Data.AppIdentityDbContext;
using Techcart.Models;

namespace Techcart.Controllers
{
    [Authorize(Policy ="RequiredUserRole")]
    public class CartController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public CartController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // Add item to cart or update quantity
        public async Task<IActionResult> AddCart(string id, string name, decimal price, int quantity=1)

        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.Name == name);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                Cart cart= new Cart {
                    Name = name,
                    Price = price,
                    Quantity = quantity,
                    UserId = id
                };
                await _context.Carts.AddAsync(cart);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Remove item or decrease quantity
        public async Task<IActionResult> RemoveCart(int id)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id );
            if (cart != null)
            {
                if (cart.Quantity > 1)
                {
                    cart.Quantity--;
                }
                else
                {
                    _context.Carts.Remove(cart);
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Display cart items
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.Carts.ToListAsync();
            return View(cartItems);
        }
    }
}