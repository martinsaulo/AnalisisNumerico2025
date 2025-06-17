using AnalisisNumericoWebApp.Entities;
using System.Collections;
using System.Drawing.Drawing2D;

namespace AnalisisNumericoWebApp.Services
{
    public class CurveFitting : ICurveFitting
    {
        public CurveFittingResponseDTO SolveCurveFitting(CurveFittingRequestDTO request)
        {
            if (request.Points.Count < 2)
            {
                throw new ArgumentException("Se deben ingresar como mínimo 2 puntos.");
            }
            if (request.Tolerance < 0)
            {
                throw new ArgumentException("La tolerancia debe ser positiva.");
            }
            if (request.Grade < 0)
            {
                throw new ArgumentException("El grado debe ser positiva.");
            }

            if (request.Method == "linear_regression")
            {
                return LinearRegression(request);
            }
            if (request.Method == "polinomial_regression")
            {
                return PolinomialRegression(request);
            }

            throw new ArgumentException("Método incompatible.");
        }
        public CurveFittingResponseDTO LinearRegression(CurveFittingRequestDTO request)
        {
            int n = request.Points.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumPowX = 0;
            foreach (var point in request.Points)
            {
                sumX += point[0];
                sumY += point[1];
                sumXY += point[0] * point[1];
                sumPowX += Math.Pow(point[0], 2);
            }

            double a1 = (n * sumXY - sumX * sumY) / (n * sumPowX - Math.Pow(sumX, 2));
            double a0 = (sumY / n) - a1 * (sumX / n);
            double st = 0, sr = 0;

            foreach (var point in request.Points)
            {
                st += Math.Pow((sumY / n - point[1]), 2);
                sr += Math.Pow((a1 * point[0] + a0 - point[1]), 2);
            }

            double correlationCoefficient = Math.Sqrt((st - sr) / st) * 100;
            return new CurveFittingResponseDTO()
            {
                CorrelationCoefficient = correlationCoefficient,
                Function = $"y = {double.Round(a1, 2)}x + {double.Round(a0, 2)}",
                IsEffective = correlationCoefficient > request.Tolerance
            };
        }
        public CurveFittingResponseDTO PolinomialRegression(CurveFittingRequestDTO request)
        {
            var matrix = GeneratePolinomialMatrix(request.Grade, request.Points);
            var result = SolveSystemOfEquations.GaussJordanMethod(request.Grade + 1, matrix.ToList());

            string function = string.Empty, sign = string.Empty;

            for (int i = 0; i < result.Count; i++)
            {
                double ai = Math.Round(result[i], 4);
                if (i == 0 && ai != 0)
                {
                    function = $"{ai}";
                }
                else if (i == 1 && ai != 0)
                {
                    function = $"{ai}x {sign}" + function;
                }
                else
                {
                    if (ai != 0)
                    {
                        function = $"{ai}x^{i} {sign}" + function;
                    }
                }
                sign = (ai > 0) ? "+" : string.Empty;
            }

            double correlationCoefficient = CalcCorrelationCoefficient(request.Points, result);
            return new CurveFittingResponseDTO()
            {
                CorrelationCoefficient = correlationCoefficient,
                Function = function,
                IsEffective = correlationCoefficient > request.Tolerance
            };
        }
        public List<DoubleVector> GeneratePolinomialMatrix(int grade, List<List<double>> points)
        {
            int dimension = grade + 1;
            var matrix = new List<List<double>>();

            for (int i = 0; i < dimension; i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < dimension + 1; j++)
                {
                    row.Add(0);
                }
                matrix.Add(row);
            }

            double x, y;
            foreach (var point in points)
            {
                x = point[0];
                y = point[1];
                for (int row = 0; row < dimension; row++)
                {
                    for (int col = 0; col < dimension; col++)
                    {
                        matrix[row][col] += Math.Pow(x, row + col);
                    }
                    matrix[row][dimension] += y * Math.Pow(x, row);
                }
            }

            var ret = from vector in matrix select new DoubleVector(vector);
            return ret.ToList();
        }
        public double CalcCorrelationCoefficient(List<List<double>> points, List<double> result)
        {
            double x, y;
            double total, sumY = 0, sr = 0, st = 0;
            sumY = points.Sum(x => x[1]);
            foreach (var point in points)
            {
                x = point[0];
                y = point[1];
                total = 0;
                for (int i = 0; i < result.Count; i++)
                {
                    total += result[i] * Math.Pow(x, i);
                }
                sr += Math.Pow(total - y, 2);
                st += Math.Pow((sumY / points.Count) - y, 2);
            }
            return Math.Sqrt((st - sr) / st) * 100;
        }

    }
}
