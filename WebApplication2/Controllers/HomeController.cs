using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techcart.Data.AppIdentityDbContext;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly AppIdentityDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(AppIdentityDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index(string searchtitle)
    {
        var products = _context.Products.ToList();
        if (!string.IsNullOrEmpty(searchtitle))
        {
            products = products.Where(b => b.Name.Contains(searchtitle)).ToList();
        }
        return View(products);

    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
