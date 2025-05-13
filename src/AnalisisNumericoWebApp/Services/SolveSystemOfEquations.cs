using AnalisisNumericoWebApp.Entities;
using System.Collections;
using System.Numerics;

namespace AnalisisNumericoWebApp.Services
{
    public class SolveSystemOfEquations : ISolveSystemOfEquations
    {
        public Vector<double> SolveSystem(SystemOfEquationsRequestDTO request)
        {
            if(request.Dimension < 2)
                throw new ArgumentException("La dimensión de la matriz es de mínimo 2x2.");

            if (request.Iterations <= 0)
                throw new ArgumentException("El mínimo de iteraciones es 1.");

            if (request.Tolerance <= 0)
                throw new ArgumentException("La tolerancia debe ser un número positivo.");


            if (request.Method == "gauss_jordan")
                return GaussJordanMethod(request);

            if(request.Method == "gauss_seidel")
                return GaussSeidelMethod(request);

            throw new ArgumentException("Método incompatible.");
        }
        public Vector<double> GaussJordanMethod(SystemOfEquationsRequestDTO request)
        {

            for (int rowDiag = 0; rowDiag < request.Dimension; rowDiag++)
            {
                double diagonalCoefficient = request.Matrix[rowDiag][rowDiag];

                if(diagonalCoefficient == 0)
                {
                    throw new DivideByZeroException("Hay un 0 en la diagonal principal.");
                }

                request.Matrix[rowDiag] /= diagonalCoefficient;

                for (int row = 0; row < request.Dimension; row++)
                {
                    if(rowDiag != row)
                    {
                        double rowCoefficient = request.Matrix[row][rowDiag];
                        request.Matrix[row] -= rowCoefficient * request.Matrix[rowDiag];
                    }
                }
            }

            var results = from row in request.Matrix select row[request.Dimension];

            return new Vector<double>(results.ToArray());
        }
        public Vector<double> GaussSeidelMethod(SystemOfEquationsRequestDTO request)
        {
            int count = 0;
            bool isSolution = false;
            var resultVector = Vector<double>.Zero;
            var prevVector = Vector<double>.Zero;
            double error, result, coefficient;
            int sameResultCount;

            while (!isSolution)
            {
                if (count > request.Iterations)
                    throw new ArgumentException("Se llego al limite de iteraciones.");

                if (count > 0)
                    prevVector = resultVector;
                
                for(int row = 0; row < request.Dimension; row++)
                {
                    result = request.Matrix[row][request.Dimension];
                    coefficient = request.Matrix[row][row];

                    for (int col = 0; col < request.Dimension; col++)
                    {
                        if(col != row)
                            result -= request.Matrix[row][col] * resultVector[col];
                    }

                    coefficient = result / coefficient;
                    resultVector = resultVector.WithElement(row, coefficient);
                }

                error = 0;
                sameResultCount = 0;

                for(int i = 0; i < request.Dimension; i++)
                {
                    error = double.Abs( (resultVector[i] - prevVector[i]) / resultVector[i]);
                    if(error < request.Tolerance)
                        sameResultCount++;
                }

                isSolution = sameResultCount == request.Dimension;
                count++;
            }
            
            return resultVector;
        }
    }
}
