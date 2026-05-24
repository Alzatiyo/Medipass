using Aplication.Ports.Out;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure.Adapters.Rest;

public class InsuranceServiceAdapter : IInsuranceServicePort
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public InsuranceServiceAdapter(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // Leer la URL del microservicio de seguros desde appsettings.json
        _baseUrl = configuration["Services:InsuranceService"] ?? "http://localhost:5002";
    }

    public async Task<InsuranceStatus> ValidateProcedureAsync(
        string insuranceNumber,
        string procedureCode)
    {
        try
        {
            var requestBody = new
            {
                InsuranceNumber = insuranceNumber,
                ProcedureCode = procedureCode
            };

            var url = $"{_baseUrl.TrimEnd('/')}/api/insurance/validate";
            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return InsuranceStatus.Rejected;
            }

            var result = await response.Content.ReadFromJsonAsync<InsuranceValidationResponse>();

            if (result == null)
            {
                return InsuranceStatus.Rejected;
            }

            // Si es aprobado en MS-Insurance, lo marcamos como Aprobado en MS-AgendaHub
            if (result.Approved)
            {
                return InsuranceStatus.Approved;
            }

            // Si el estado explícito es que no está cubierto por el plan
            if (result.Status == nameof(InsuranceStatus.ProcedureNotCovered))
            {
                return InsuranceStatus.ProcedureNotCovered;
            }

            // Para planes inactivos o pólizas inexistentes
            return InsuranceStatus.Rejected;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de red al conectar con MS-Insurance: {ex.Message}");
            return InsuranceStatus.Rejected;
        }
    }

    private class InsuranceValidationResponse
    {
        public bool Approved { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
