namespace AnalisisNumericoWebApp.Entities
{
    public class CurveFittingResponseDTO
    {
        public string? Function { get; set; }
        public double CorrelationCoefficient { get; set; }
        public bool IsEffective { get; set; }
    }
}
