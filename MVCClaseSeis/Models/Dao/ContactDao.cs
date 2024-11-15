using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MVCClaseSeis.Models.Dto;


namespace MVCClaseSeis.Models
{
    public class ContactDAO
    {
        private readonly HttpClient _httpClient; // HTTP client for sending requests to the API
        private readonly AuthService _authService; // Service for obtaining authorization token
        private readonly string _apiUrl = "https://saacapps.com/payout/contact.php"; // Base URL for the contact API

        // Constructor to initialize HttpClient and AuthService dependencies
        public ContactDAO(HttpClient httpClient, AuthService authService)
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

        // Retrieves a list of all contacts from the API
        public async Task<List<ContactDTO>> GetAllContactsAsync()
        {
            await SetAuthorizationHeader(); // Set authorization header
            var response = await _httpClient.GetAsync(_apiUrl); // Send GET request to the API
            response.EnsureSuccessStatusCode(); 

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ContactDTO>>(responseData); 
        }

        // Sends a new contact to the API to be created
        public async Task<bool> CreateContactAsync(ContactDTO newContact)
        {
            await SetAuthorizationHeader(); // Set authorization header

            // Define the contact data structure expected by the API
            var contactData = new
            {
                first_name = newContact.First_Name,  
                last_name = newContact.Last_Name,    
                email = newContact.Email,            
                phone = newContact.Phone             
            };

            // Serialize the contact data to JSON format
            var jsonContent = JsonConvert.SerializeObject(contactData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // Set content type to JSON

            // Send POST request to the API to create a new contact
            var response = await _httpClient.PostAsync(_apiUrl, content);

            var responseData = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

        // Retrieves a contact by email from the API
        public async Task<ContactDTO> GetContactByEmailAsync(string email)
        {
            await SetAuthorizationHeader(); // Set authorization header
            var response = await _httpClient.GetAsync($"{_apiUrl}?email={email}"); // Send GET request with email parameter

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ContactDTO>(responseData);
            }
            else
            {
                return null;
            }
        }
    }
}
