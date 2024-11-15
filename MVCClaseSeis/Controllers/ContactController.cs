using System.Threading.Tasks;
using System.Web.Mvc;
using MVCClaseSeis.Models.Dto;
using MVCClaseSeis.Models;

public class ContactController : Controller
{
    // Dependency injection for the ContactDAO instance
    private readonly ContactDAO _contactDAO;

        // Constructor to initialize the ContactDAO dependency
        public ContactController(ContactDAO contactDAO)
        {
            _contactDAO = contactDAO;
        }

        // Action to display all contacts in the Index view
        public async Task<ActionResult> Index()
        {
            var contacts = await _contactDAO.GetAllContactsAsync();
            return View(contacts);
        }

        // Action to display the Create view where a user can add a new contact
        public ActionResult Create()
        {
            return View();
        }

        // POST Action for creating a new contact with form validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactDTO contact)
        {
            // Checks if the model is valid
            if (ModelState.IsValid)
            {
                // Calls the DAO to add the new contact
                var result = await _contactDAO.CreateContactAsync(contact);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "The system could not find the contact.";
                }
            }
            return View(contact);
        }

        // Action to display the Search view
        public ActionResult Search()
        {
            return View();
        }

        // GET Action to search for a contact by email
        [HttpGet]
        public async Task<ActionResult> SearchByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "You must type a valid email.";
                return View("Search");
            }

            var contact = await _contactDAO.GetContactByEmailAsync(email);

            if (contact != null)
            {
                return View("Details", contact);
            }
            else
            {
                ViewBag.Error = "The system could not find the contact.";
                return View("Search");
            }
        }

        // Action to display contact details by email
        public async Task<ActionResult> Details(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "You must type a valid email";
                return View("Index");
            }

            var contact = await _contactDAO.GetContactByEmailAsync(email);

            if (contact != null)
            {
                return View("Details", contact);
            }
            else
            {
                ViewBag.Error = "The system could not find the contact.";
                return View("Index");
            }
        }
}
