using AnalisisNumericoWebApp.Entities;
using AnalisisNumericoWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalisisNumericoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICalcFunctionRoot _calcFunctionRoot;
        private readonly ISolveSystemOfEquations _solveSystemOfEquations;
        public HomeController(ICalcFunctionRoot calcFunctionRoot, ISolveSystemOfEquations solveSystemOfEquations)
        {
            _calcFunctionRoot = calcFunctionRoot;
            _solveSystemOfEquations = solveSystemOfEquations;
        }
        public IActionResult CalcFunctionRoot(RootCalcRequestDTO request)
        {
            try
            {
                ViewBag.Response = _calcFunctionRoot.CalculateRoot(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            
            return View("~/Views/Home/CalcFunctionRootView.cshtml");
        }
        public IActionResult SolveSystemOfEquations()
        {
            try
            {
                ViewBag.Response = _solveSystemOfEquations;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("~/Views/Home/CalcFunctionRootView.cshtml");
        }
        public IActionResult CalcFunctionRootView()
        {
            return View();
        }
        public IActionResult SolveSystemOfEquationsView()
        {
            return View();
        }
    }
}
