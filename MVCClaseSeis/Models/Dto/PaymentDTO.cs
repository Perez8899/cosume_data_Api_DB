// PaymentRequestDTO.cs
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
    public bool Success { get; set; }
    public string Message { get; set; } // Success or error message
    public DateTime PaymentDate { get; set; } // Date of payment
}
