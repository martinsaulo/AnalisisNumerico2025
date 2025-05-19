using AnalisisNumericoWebApp.Entities;
using System.Numerics;

namespace AnalisisNumericoWebApp.Services
{
    public interface ISolveSystemOfEquations
    {
        public List<double> SolveSystem(SystemOfEquationsRequestDTO request);
    }
}
