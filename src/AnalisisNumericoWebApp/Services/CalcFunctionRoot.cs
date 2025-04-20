using AnalisisNumericoWebApp.Entities;
using Calculus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnalisisNumericoWebApp.Services
{
    public class CalcFunctionRoot : ICalcFunctionRoot
    {
        private Calculo _calc = new Calculo();
        private double funcR { get; set; }
        private double funcL { get; set; }

        public RootCalcResponseDTO CalculateRoot(RootCalcRequestDTO request)
        {
            if (request.LeftLimit > request.RightLimit && request.Method != "newton_raphson")
            {
                throw new ArgumentException("El limite izquierdo no puede ser mayor o igual al limite derecho.");
            }

            if (request.Iterations <= 0)
            {
                throw new ArgumentException("El minimo de iteraciones es 1.");
            }

            if (request.Tolerance <= 0)
            {
                throw new ArgumentException("La tolerancia debe ser un número positivo.");
            }

            if (request.Function == null || !_calc.Sintaxis(request.Function, 'x'))
            {
                throw new ArgumentException("La función dada es invalida.");
            }

            funcR = _calc.EvaluaFx(request.RightLimit);
            funcL = _calc.EvaluaFx(request.LeftLimit);

            if (funcL * funcR == 0)
            {
                return new RootCalcResponseDTO()
                {
                    Result = (funcL == 0) ? funcL : funcR,
                    RelativeError = 0,
                    Iterations = 1,
                    Message = "Uno de los limites es raíz."
                };
            }

            switch (request.Method)
            {
                default:
                    throw new ArgumentException("Método invalido");

                case "bisection":
                case "false_position":
                    return ClosedMethod(request);

                case "secant":
                case "newton_raphson":
                    return OpenMethod(request);
            }
        }

        private double CalculateXr(RootCalcRequestDTO request)
        {
            switch (request.Method)
            {
                default:
                    throw new ArgumentException("Método invalido");


                case "bisection":
                    return (request.LeftLimit + request.RightLimit) / 2;


                case "newton_raphson":
                    double Dx = _calc.Dx(request.LeftLimit);
                    funcR = _calc.EvaluaFx(request.RightLimit);
                    funcL = _calc.EvaluaFx(request.LeftLimit);

                    if (Dx <= request.Tolerance) 
                       throw new ArgumentException("La derivada es menor a la tolerancia.");
                    
                    return request.LeftLimit - (funcL / Dx);


                case "secant":
                case "false_position":
                    funcR = _calc.EvaluaFx(request.RightLimit);
                    funcL = _calc.EvaluaFx(request.LeftLimit);
                    return (funcR * request.LeftLimit - funcL * request.RightLimit) / (funcR - funcL);
                }

        }

        private RootCalcResponseDTO OpenMethod(RootCalcRequestDTO request)
        {
            if(double.Abs(funcL) <= request.Tolerance)
            {
                return new RootCalcResponseDTO()
                {
                    Result = request.LeftLimit,
                    RelativeError = 0,
                    Iterations = 1,
                    Message = "Uno de los limites es raíz."
                };
            }

            if(request.Method == "newton_raphson" && double.Abs(funcR) <= request.Tolerance)
            {
                return new RootCalcResponseDTO()
                {
                    Result = request.RightLimit,
                    RelativeError = 0,
                    Iterations = 1,
                    Message = "El punto inicial es raíz."
                };
            }

            double xr, funcXr, error, prevXr;
            int i = 0;
            xr = prevXr = error = 0;

            for (i = 1; i < request.Iterations; i++)
            {
                xr = CalculateXr(request);
                funcXr = _calc.EvaluaFx(xr);
                error = double.Abs((xr - prevXr) / xr);

                if (xr == double.NaN)
                    throw new ArgumentException("El metodo diverge");

                if(double.Abs(funcXr) < request.Tolerance || error <= request.Tolerance)
                {
                    return new RootCalcResponseDTO()
                    {
                        Result = xr,
                        RelativeError = error,
                        Iterations = i,
                        Message = "Finaliza por encontrar la raíz."
                    };
                }

                if(request.Method == "newton_raphson")
                {
                    request.LeftLimit = xr;
                }
                else
                {
                    request.LeftLimit = request.RightLimit;
                    request.RightLimit = xr;
                }

                prevXr = xr;
            }

            return new RootCalcResponseDTO()
            {
                Result = xr,
                RelativeError = error,
                Iterations = i,
                Message = "Finaliza por llegar al limite de iteraciones."
            };
        }

        private RootCalcResponseDTO ClosedMethod(RootCalcRequestDTO request)
        {
            if (funcL * funcR > 0)
            {
                throw new ArgumentException("No existe una raíz entre los limites dados.");
            }

            double xr, funcXr, error, prevXr;
            int i = 0;
            xr = prevXr = error = 0;

            for (i = 1; i < request.Iterations; i++)
            {
                xr = CalculateXr(request);
                error = double.Abs((xr - prevXr) / xr);
                funcR = _calc.EvaluaFx(request.RightLimit);
                funcL = _calc.EvaluaFx(request.LeftLimit);
                funcXr = _calc.EvaluaFx(xr);

                if (double.Abs(funcXr) < request.Tolerance || error <= request.Tolerance)
                {
                    return new RootCalcResponseDTO()
                    {
                        Result = xr,
                        RelativeError = error,
                        Iterations = i,
                        Message = "Finaliza por encontrar la raíz."
                    };
                }
                else
                {
                    if (funcL * funcXr > 0)
                        request.LeftLimit = xr;
                    else
                        request.RightLimit = xr;

                    prevXr = xr;
                }
            }

            return new RootCalcResponseDTO()
            {
                Result = xr,
                RelativeError = error,
                Iterations = i,
                Message = "Finaliza por llegar al limite de iteraciones."
            };
        }
    }
}
