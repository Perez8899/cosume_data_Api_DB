using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MVCClaseSeis.Models
{
    public class AuthService
    {
        private readonly HttpClient _httpClient; // HTTP client to send requests to the auth API
        private readonly string _authUrl = "https://saacapps.com/payout/auth.php"; // Base URL for authentication

        // Constructor to initialize the HttpClient dependency
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to retrieve the authentication token
        public async Task<string> GetAuthTokenAsync()
        {
            // Encode the credentials in Base64 format
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hyh:hyhector"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials); //tenia Basic
            var response = await _httpClient.PostAsync(_authUrl, null);
            response.EnsureSuccessStatusCode(); // Ensure the response was successful
            var responseData = await response.Content.ReadAsStringAsync();
            // Deserialize the JSON response into a TokenResponse object
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);

            // Return the token from the response
            return tokenResponse.Token;
        }
    }

    // Class to represent the token response from the auth API
    public class TokenResponse
    {
        public string Token { get; set; } // Property to hold the token string
    }
}
