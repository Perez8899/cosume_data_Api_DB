// PaymentDAO.cs
using MVCClaseSeis.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

public class PaymentDAO
{
        private readonly HttpClient _httpClient; // HTTP client for sending requests to the API
        private readonly AuthService _authService; // Service for obtaining authorization token
        private readonly string _apiUrl = "https://saacapps.com/payout/payout.php"; // Base URL for the contact API

        // Constructor to initialize HttpClient and AuthService dependencies
        public PaymentDAO(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // Sets the Authorization header with a Bearer token for secure requests
        private async Task SetAuthorizationHeader()
        {
            var token = await _authService.GetAuthTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Sends a new contact to the API to be created
        public async Task<bool> CreatePaymentAsync(PaymentRequestDTO newPayment)
        {
            await SetAuthorizationHeader(); // Set authorization header

            // Define the contact data structure expected by the API
            var paymentData = new
            {
                email = newPayment.Email,  
                amount = newPayment.Amount,             
            };

            // Serialize the contact data to JSON format
            var jsonContent = JsonConvert.SerializeObject(paymentData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // Set content type to JSON

            // Send POST request to the API to create a new contact
            var response = await _httpClient.PostAsync(_apiUrl, content);

            var responseData = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

    public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsAsync()
    {
        var response = await _httpClient.GetAsync("https://saacapps.com/payout/payout.php");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PaymentResponseDTO>>();
        }
        return null;
    }
}
