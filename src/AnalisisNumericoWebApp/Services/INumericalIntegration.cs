using AnalisisNumericoWebApp.Entities;

namespace AnalisisNumericoWebApp.Services
{
    public interface INumericalIntegration
    {
        double SolveIntegration(IntegrationRequestDTO request);
    }
}
