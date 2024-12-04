using MVCClaseSeis.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace MVCClaseSeis
{
    public class PaymentController : Controller
    {
        private readonly PaymentDAO _paymentDAO;
        private readonly ContactDAO _contactDAO;
        private PaymentDAO paymentDAO = new PaymentDAO();


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

                // Check if the email exists in the system
                var contact = await _contactDAO.GetContactByEmailAsync(payment.Email);

                if (contact == null)
                {
                    // If the email does not exist, show error message
                    TempData["ErrorMessage"] = "The email does not exist in the system.";
                    return View(payment);
                }

                // If the email exists, create the payment
                var paymentResponse = await _paymentDAO.CreatePaymentAsync(payment);
                if (paymentResponse)
                {
                    TempData["SuccessMessage"] = "Payment successfully created!";
                    return RedirectToAction("Create");
                }
                else
                {
                    ViewBag.Error = "The system could not create the payment.";
                }
            }

            return View(payment);
        }

        //----------VIEWS REPORT PAYOUTS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Report()
        {
            return View(paymentDAO.ReadPayments());
        }
    }
}
