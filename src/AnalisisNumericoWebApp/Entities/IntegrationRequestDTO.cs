namespace AnalisisNumericoWebApp.Entities
{
    public class IntegrationRequestDTO
    {
        public string? Method { get; set; }
        public string? Function { get; set; }
        public int Subintervals { get; set; }
        public double LeftLimit { get; set; }
        public double RightLimit { get; set; }
    }
}
