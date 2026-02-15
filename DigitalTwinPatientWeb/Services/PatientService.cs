using DigitalTwinPatientWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace DigitalTwinPatientWeb.Services
{
    public class PatientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PatientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetPatientNameFromJwtAsync(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(jwtToken);
                var patientId = int.Parse(jwt.Claims.First(c => c.Type == "userId").Value);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7070/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var response = await client.GetAsync($"api/patient/{patientId}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonSerializer.Deserialize<PatientModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return $"{patient.Surname} {patient.Name} {patient.Patronymic}";
            }
            catch
            {
                return null;
            }
        }
    }
}