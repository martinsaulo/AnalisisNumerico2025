namespace AnalisisNumericoWebApp.Entities
{
    public class CurveFittingRequestDTO
    {
        public List<List<double>>? Points { get; set; }
        public string? PointsJSON { get; set; }
        public double Tolerance { get; set; }
        public string? Method { get; set; }
        public int Grade { get; set; }
    }
}
