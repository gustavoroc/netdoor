using Client.Models;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Login(LoginModel model)
    {
        try
        {
            // Log dos dados sendo enviados
            Console.WriteLine($"Sending login request with email: {model.Email}");

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(model);
            Console.WriteLine($"Request JSON: {jsonContent}");

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5285/api/Auth/login");
            request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Log dos headers
            Console.WriteLine("Request Headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            var response = await _httpClient.SendAsync(request);

            // Log da resposta completa
            Console.WriteLine($"Response Status: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<string>();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during login: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return false;
        }
    }
}