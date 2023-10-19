using System.Diagnostics;
using general.purpose.poc.Models.Web;
using Microsoft.AspNetCore.Mvc;

namespace general.purpose.poc.Controllers;

/// <summary>
/// Home controller class.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">logger instance.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Displays home index page to user.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays privacy index page to user.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays error page to user.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
