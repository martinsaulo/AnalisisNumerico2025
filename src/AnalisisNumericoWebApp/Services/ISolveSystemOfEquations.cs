using AnalisisNumericoWebApp.Entities;
using System.Numerics;

namespace AnalisisNumericoWebApp.Services
{
    public interface ISolveSystemOfEquations
    {
        public Vector<double> SolveSystem(SystemOfEquationsRequestDTO request);
    }
}
