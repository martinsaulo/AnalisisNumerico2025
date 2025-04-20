namespace AnalisisNumericoWebApp.Entities
{
    public class RootCalcResponseDTO
    {
        public double Result { get; set; }
        public double RelativeError { get; set; }
        public int Iterations { get; set; }
        public String? Message { get; set; }
    }
}
