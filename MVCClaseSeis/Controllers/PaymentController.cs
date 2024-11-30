// PaymentController.cs
using Microsoft.AspNetCore.Mvc;
using MVCClaseSeis.Models;
using MVCClaseSeis.Models.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;

public class PaymentController : Controller
{
    private readonly PaymentDAO _paymentDAO;
    private readonly ContactDAO _contactDAO;

    public PaymentController(PaymentDAO paymentDAO, ContactDAO contactDAO)
    {
        _paymentDAO = paymentDAO;
        _contactDAO = contactDAO;

    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(PaymentRequestDTO payment)
    {
        // Checks if the model is valid
        if (ModelState.IsValid)
        {
            // Check if the amount is valid (positive number)
            if (payment.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be a positive number.");
                return View(payment);
            }

            // Verificar si el correo electrónico existe en el sistema
            var contact = await _contactDAO.GetContactByEmailAsync(payment.Email);

            if (contact == null)
            {
                // Si el correo electrónico no existe, mostrar mensaje de error
                TempData["ErrorMessage"] = "The email does not exist in the system.";
                return View(payment);
            }

            // Si el correo existe, crear el pago
            var paymentResponse = await _paymentDAO.CreatePaymentAsync(payment);
            if (paymentResponse)
            {
                // Si el pago fue exitoso, mostrar mensaje de éxito
                TempData["SuccessMessage"] = "Payment successfully created!";
                return RedirectToAction("Create"); // Redirigir para mostrar el mensaje de éxito
            }
            else
            {
                // Si no se pudo crear el pago, mostrar mensaje de error
                ViewBag.Error = "The system could not create the payment.";
            }
        }

        return View(payment);
    }
}
