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
        private readonly ICurveFitting _curveFitting;
        private readonly INumericalIntegration _numericalIntegration;
        public HomeController
            (
            ICalcFunctionRoot calcFunctionRoot,
            ISolveSystemOfEquations solveSystemOfEquations,
            ICurveFitting curveFitting,
            INumericalIntegration numericalIntegration
            )
        {
            _calcFunctionRoot = calcFunctionRoot;
            _solveSystemOfEquations = solveSystemOfEquations;
            _curveFitting = curveFitting;
            _numericalIntegration = numericalIntegration;
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
        public IActionResult SolveSystemOfEquations([FromForm] string request)
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

            if (requestDTO != null)
            {
                ViewBag.PrevDimension = requestDTO.Dimension;
                ViewBag.PrevMatrix = requestDTO.Matrix;
            }


            return View("~/Views/Home/SolveSystemOfEquationsView.cshtml");
        }
        public IActionResult NumericalIntegration(IntegrationRequestDTO request)
        {
            try
            {
                ViewBag.Response = _numericalIntegration.SolveIntegration(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("~/Views/Home/NumericalIntegrationView.cshtml");
        }
        public IActionResult CurveFitting(CurveFittingRequestDTO request)
        {
            request.Points = JsonConvert.DeserializeObject<List<List<double>>>(request.PointsJSON);
            try
            {
                ViewBag.Response = _curveFitting.SolveCurveFitting(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("~/Views/Home/CurveFittingView.cshtml");
        }
        public IActionResult CalcFunctionRootView()
        {
            return View();
        }
        public IActionResult SolveSystemOfEquationsView()
        {
            return View();
        }
        public IActionResult NumericalIntegrationView()
        {
            return View();
        }
        public IActionResult CurveFittingView()
        {
            return View();
        }
    }
}
