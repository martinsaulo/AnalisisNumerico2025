using AnalisisNumericoWebApp.Entities;

namespace AnalisisNumericoWebApp.Services
{
    public interface ICalcFunctionRoot
    {
        RootCalcResponseDTO CalculateRoot(RootCalcRequestDTO request);
    }
}
