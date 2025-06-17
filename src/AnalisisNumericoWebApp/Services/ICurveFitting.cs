using AnalisisNumericoWebApp.Entities;

namespace AnalisisNumericoWebApp.Services
{
    public interface ICurveFitting
    {
        CurveFittingResponseDTO SolveCurveFitting(CurveFittingRequestDTO request);
    }
}
