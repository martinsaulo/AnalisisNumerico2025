using AnalisisNumericoWebApp.Entities;
using AnalisisNumericoWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

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
        [HttpPost()]
        public IActionResult SolveSystemOfEquations([FromForm]string request)
        {
            var requestDTO = JsonConvert.DeserializeObject<SystemOfEquationsRequestDTO>(request);
            try
            {
                ViewBag.Response = _solveSystemOfEquations.SolveSystem(requestDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            if(requestDTO != null)
            {
                ViewBag.PrevDimension = requestDTO.Dimension;
                ViewBag.PrevMatrix = requestDTO.Matrix;
            }


            return View("~/Views/Home/SolveSystemOfEquationsView.cshtml");
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
