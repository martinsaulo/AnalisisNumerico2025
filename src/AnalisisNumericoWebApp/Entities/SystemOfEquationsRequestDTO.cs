using System.Numerics;

namespace AnalisisNumericoWebApp.Entities
{
    public class SystemOfEquationsRequestDTO
    {
        public int Dimension { get; set; }
        public List<Vector<double>> Matrix { get; set; }
        public List<double>? Results { get; set; }
        public string? Method { get; set; }
        public double Tolerance { get; set; }
        public int Iterations { get; set; }
    }
}
