using MVCClaseSeis.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
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

    public PaymentDAO()
    {
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
        if (response.IsSuccessStatusCode)//--------------------------------------
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponseDTO>(responseData);

            // Verify that the contact_id is not 0
            if (paymentResponse.contact_id == 0)
            {
                Console.WriteLine("Error: Contact ID is 0, payment will not be saved.");
                return false; // Si el contact_id es 0, no guardar el pago
            }


            // // Save the payment to the database
            await SavePaymentToDatabaseAsync(paymentResponse.contact_id, paymentResponse.Amount, paymentResponse.Status);

            return true;
        }
        return false;
    }

 
    //------------Views Report------------------------------------------------------
    public List<PaymentResponseDTO> ReadPayments()
    {
        List<PaymentResponseDTO> payments = new List<PaymentResponseDTO>();
        using (MySqlConnection cnx = Cnx.getCnx())
        {
            try
            {
                cnx.Open();
                string query = "select * from Payouts";

                MySqlCommand cmd = new MySqlCommand(query, cnx);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentResponseDTO payment = new PaymentResponseDTO
                        {
                            Id = reader.GetInt32("Id"),
                            contact_id = reader.GetInt32("contact_id"),
                            Amount = reader.GetDecimal("Amount"),
                            Status = reader.GetString("Status"),
                            Created_at = reader.GetDateTime("Created_at"),

                        };
                        payments.Add(payment);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        return payments;
    }
    //-------------------------------------------------------INSERT DATA IN DB---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public async Task<bool> SavePaymentToDatabaseAsync(int contact_id, decimal amount, string status)
    {
        if (contact_id == 0)
        {
            Console.WriteLine("Error: Contact ID is 0, not saving payment.");
            return false; // Do not proceed with insert if contact_id is 0
        }
        using (MySqlConnection cnx = Cnx.getCnx())
        {
            try
            {
                cnx.Open();
                string query = @"
                INSERT INTO payouts (contact_id, amount, status, created_at) 
                VALUES (@contact_id, @Amount, @Status, @CreatedAt)";

                    MySqlCommand command = new MySqlCommand(query, cnx);
                    command.Parameters.AddWithValue("@contact_id", contact_id);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;


                }
            catch (Exception ex)
            {
                // MError handling
                Console.WriteLine($"Error saving payment to database: {ex.Message}");
                return false;
            }
        }
    }
}



