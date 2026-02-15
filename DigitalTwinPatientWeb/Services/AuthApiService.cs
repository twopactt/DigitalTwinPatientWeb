using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;
using System.Text.Json;

namespace DigitalTwinPatientWeb.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> LoginAsync(string login, string password)
        {
            try
            {
                var payload = new
                {
                    Login = login,
                    Password = password
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _http.PostAsync("api/auth/patient/login", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse( json );

                return doc.RootElement.GetProperty("token").GetString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
