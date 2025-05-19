using AnalisisNumericoWebApp.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace AnalisisNumericoWebApp.Services
{
    public class SolveSystemOfEquations : ISolveSystemOfEquations
    {
        public List<double> SolveSystem(SystemOfEquationsRequestDTO request)
        {
            if(request.Dimension < 2)
                throw new ArgumentException("La dimensión de la matriz es de mínimo 2x2.");

            if (request.Iterations <= 0)
                throw new ArgumentException("El mínimo de iteraciones es 1.");

            if (request.Tolerance <= 0)
                throw new ArgumentException("La tolerancia debe ser un número positivo.");

            var matrix = from vector in request.Matrix select new DoubleVector(vector);

            if (request.Method == "gauss_jordan")
                return GaussJordanMethod(request, matrix.ToList());

            if(request.Method == "gauss_seidel")
                return GaussSeidelMethod(request, matrix.ToList());

            throw new ArgumentException("Método incompatible.");
        }
        public List<double> GaussJordanMethod(SystemOfEquationsRequestDTO request, List<DoubleVector> matrix)
        {

            for (int rowDiag = 0; rowDiag < request.Dimension; rowDiag++)
            {
                double diagonalCoefficient = matrix[rowDiag].Get(rowDiag);

                if(diagonalCoefficient == 0)
                {
                    throw new DivideByZeroException("Hay un 0 en la diagonal principal.");
                }

                matrix[rowDiag] /= diagonalCoefficient;

                for (int row = 0; row < request.Dimension; row++)
                {
                    if(rowDiag != row)
                    {
                        double rowCoefficient = matrix[row].Get(rowDiag);
                        matrix[row] -= matrix[rowDiag] * rowCoefficient;
                    }
                }
            }

            var results = from row in matrix select row.Get(request.Dimension);

            return results.ToList();
        }
        public List<double> GaussSeidelMethod(SystemOfEquationsRequestDTO request, List<DoubleVector> matrix)
        {
            int count = 0;
            bool isSolution = false;
            var resultVector = DoubleVector.GetNullVector(request.Dimension);
            var prevVector = DoubleVector.GetNullVector(request.Dimension);
            double error, result, coefficient, difference;
            int sameResultCount;

            while (!isSolution)
            {
                if (count > request.Iterations)
                {
                    ValidationException ex = new ValidationException("Se llego al limite de iteraciones.");
                    ex.Data.Add("LastValue", resultVector);
                }
                    
                if (count > 0)
                    prevVector = resultVector;
                
                for(int row = 0; row < request.Dimension; row++)
                {
                    result = matrix[row].Get(request.Dimension);
                    coefficient = matrix[row].Get(row);

                    for (int col = 0; col < request.Dimension; col++)
                    {
                        if(col != row)
                            result -= matrix[row].Get(col) * resultVector.Get(col);
                    }

                    coefficient = result / coefficient;
                    resultVector = resultVector.WithElement(row, coefficient);
                }

                error = 0;
                sameResultCount = 0;

                for(int i = 0; i < request.Dimension; i++)
                {
                    difference = double.Abs(resultVector.Get(i) - prevVector.Get(i));
                    error = (resultVector.Get(i) != 0) ? difference / resultVector.Get(i) : difference;

                    if (error < request.Tolerance)
                        sameResultCount++;
                }

                isSolution = sameResultCount == request.Dimension;
                count++;
            }
            
            return resultVector.ToList();
        }
    }
}
