using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GarageMvc.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using GarageDomain.Entities;

namespace GarageMvc.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    [Route("")]
    public IActionResult RedirectToFeatured()
    {
        return Redirect("/Featured-Products");
    }


    [Route("Featured-Products")]
    public async Task<IActionResult> Index()
    {
        var result = await _httpClient.GetAsync("Home/GetAllProducts");

        if (result.IsSuccessStatusCode)
        {
            var allProducts = await result.Content.ReadFromJsonAsync<List<Products>>();
            return View(allProducts);
        }

        return View(new List<Products>()); 
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
