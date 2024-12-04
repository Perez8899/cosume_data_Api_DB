using System;
using System.ComponentModel.DataAnnotations;

public class PaymentRequestDTO
{
    [Required(ErrorMessage = "This field is required for creating a contact.")]
    [EmailAddress]
    public string Email { get; set; } // Contact to whom the payment is made
    public decimal Amount { get; set; } // Payment amount
}

// PaymentResponseDTO.cs
public class PaymentResponseDTO
{
    public int Id { get; set; }
    public int contact_id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime Created_at { get; set; }

    // Campo adicional para el nombre del contacto obtenido de la API
    public string ContactName { get; set; }
}


