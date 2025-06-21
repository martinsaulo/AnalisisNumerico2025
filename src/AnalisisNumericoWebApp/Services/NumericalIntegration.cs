using AnalisisNumericoWebApp.Entities;
using Calculus;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AnalisisNumericoWebApp.Services
{
    public class NumericalIntegration : INumericalIntegration
    {
        private readonly Calculo _calc = new Calculo();
        public double SolveIntegration(IntegrationRequestDTO request)
        {
            if (request.Subintervals < 0)
            {
                throw new ArgumentException("El número de subintervalos debe ser positivo.");
            }

            if (!_calc.Sintaxis(request.Function, 'x'))
            {
                throw new ArgumentException("La función dada es invalida.");
            }

            double xi = request.LeftLimit;
            double xd = request.RightLimit;
            int n = request.Subintervals;
            switch (request.Method)
            {
                default:
                    throw new ArgumentException("El metodo dado es invalido.");

                case "simple-trapezoid":
                    return SimpleTrapezoid(xi, xd);

                case "multiple-trapezoids":
                    return MultipleTrapezoids(xi, xd, n);

                case "simpson-1/3-single":
                    return SimpsonOneThirdSingle(xi, xd);

                case "simpson-1/3-multiple":
                    return SimpsonOneThirdMultiple(xi, xd, n);

                case "simpson-3/8":
                    return SimpsonThreeEights(xi, xd);

                case "simpson-1/3-and-3/8":
                    return SimpsonOneThirdAndThreeEights(xi, xd, n);

            }

        }
        private double SimpleTrapezoid(double xi, double xd)
        {
            return ((_calc.EvaluaFx(xi) + _calc.EvaluaFx(xd)) * (xd - xi)) / 2;
        }
        private double MultipleTrapezoids(double xi, double xd, int n)
        {
            double sum = 0;
            double h = (xd - xi) / n;
            for (int i = 1; i < n; i++)
            {
                sum += _calc.EvaluaFx(xi + h * i);
            }
            return (h / 2) * (_calc.EvaluaFx(xi) + 2 * sum + _calc.EvaluaFx(xd));
        }
        private double SimpsonOneThirdSingle(double xi, double xd)
        {
            double h = (xd - xi) / 2;
            return (h / 3) * (_calc.EvaluaFx(xi) + 4 * _calc.EvaluaFx(xi + h) + _calc.EvaluaFx(xd));
        }
        private double SimpsonOneThirdMultiple(double xi, double xd, int n)
        {
            double h = (xd - xi) / n;
            double evensSum = 0, oddsSum = 0;
            for (int i = 1; i < n; i++)
            {
                if (int.IsOddInteger(i))
                {
                    oddsSum += _calc.EvaluaFx(xi + h * i);
                }
                else
                {
                    evensSum += _calc.EvaluaFx(xi + h * i);
                }
            }
            return (h / 3) * (_calc.EvaluaFx(xi) + 2 * evensSum + 4 * oddsSum + _calc.EvaluaFx(xd));
        }
        private double SimpsonThreeEights(double xi, double xd)
        {
            double h = (xd - xi) / 3;
            return (3 * h / 8) * (_calc.EvaluaFx(xi)
                + 3 * _calc.EvaluaFx(xi + h)
                + 3 * _calc.EvaluaFx(xi + 2 * h)
                + _calc.EvaluaFx(xd));
        }
        private double SimpsonOneThirdAndThreeEights(double xi, double xd, int n)
        {
            double h = (xd - xi) / n;
            double evensSum = 0, oddsSum = 0, total = 0;
            double newXi;
            bool isSimpson38Done = false;

            for (int i = 0; i < n; i++)
            {
                if (int.IsOddInteger(n) && !isSimpson38Done)
                {
                    newXi = xi + h * (n - 3);
                    total = SimpsonThreeEights(newXi, xd);
                    n -= 3;
                    xd = newXi;
                    isSimpson38Done = true;
                }

                if (int.IsEvenInteger(i))
                {
                    oddsSum += _calc.EvaluaFx(xi + h * i);
                }
                else
                {
                    evensSum += _calc.EvaluaFx(xi + h * i);
                }
            }
            double simpson13 = (h / 3) * (_calc.EvaluaFx(xi) + 4 * evensSum + 2 * oddsSum + _calc.EvaluaFx(xd));
            return total + simpson13;
        }
    }
}
