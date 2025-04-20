using AnalisisNumericoWebApp.Entities;
using AnalisisNumericoWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalisisNumericoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICalcFunctionRoot _calcFunctionRoot;
        public HomeController(ICalcFunctionRoot calcFunctionRoot)
        {
            _calcFunctionRoot = calcFunctionRoot;
        }
        public IActionResult CalcFunctionRoot(RootCalcRequestDTO request)
        {
            try
            {
                ViewBag.Response = _calcFunctionRoot.CalculateRoot(request);
            }
            catch (ArgumentException ex)
            {
                ViewBag.Error = ex.Message;
            }
            
            return View("~/Views/Home/CalcFunctionRootView.cshtml");
        }
        public IActionResult CalcFunctionRootView()
        {
            return View();
        }
    }
}
