namespace AnalisisNumericoWebApp.Entities
{
    public class RootCalcRequestDTO
    {
        public string? Function {  get; set; }
        public string? Method { get; set; }
        public double Tolerance { get; set; }
        public int Iterations { get; set; }
        public double LeftLimit { get; set; }
        public double RightLimit { get; set; }
    }
}
