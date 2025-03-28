using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Techcart.Models;
using Techcart.Data.AppIdentityDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Techcart.Migrations;

namespace Techcart.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<OrderController> _logger;

        public OrderController(AppIdentityDbContext context, UserManager<IdentityUser> userManager, ILogger<OrderController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Amount = _context.Carts.Sum(c => c.Price * c.Quantity);
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            return RedirectToAction("PaymentSuccess", new { orderId = id });
        }


        [Authorize(Policy = "RequiredUserRole")]

        // Display the Order Summary
        public IActionResult Checkout(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                // Log the issue for debugging
                _logger.LogWarning("User ID is null or empty. User: {User}", User);
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.Carts.Where(c => c.Id == id).ToList();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var totalAmount = cartItems.Sum(c => c.Price * c.Quantity);

            var order = new Order
            {                
                UserId = userId,
                TotalAmount = totalAmount,
                PaymentMethod = "Upi",
                PaymentStatus = "Pending",
                OrderStatus = "Processing",
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.Id,
                    ProductName = c.Name,
                    Quantity = c.Quantity,
                    Price = c.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems); // Clear the cart
            _context.SaveChanges();
            
            return View("PaymentSuccess", new {id=order.OrderId});
        }


        // Process Payment and Save Order
        [HttpPost]
        public IActionResult ProcessPayment(Order order)
        {
            var cartItems = _context.Carts.Where(c => c.UserId == order.UserId).ToList();
            var userId = _userManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                order.UserId = userId;
                order.OrderDate = DateTime.Now;
                order.PaymentStatus = "Paid";
                order.OrderStatus = "Confirmed";

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Clear the cart after order is placed
                _context.Carts.RemoveRange(cartItems);
                _context.SaveChanges();

                return RedirectToAction("PaymentSuccess", new { orderId = order.OrderId });
            }
            return View("Checkout", order);
        }

        // Payment Success Page
        public IActionResult PaymentSuccess(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return NotFound();

            return View(order);
        }
    
        
    }

}
