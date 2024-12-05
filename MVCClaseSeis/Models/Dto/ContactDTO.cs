using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json;

namespace MVCClaseSeis.Models.Dto
{
    public class ContactDTO
    {
        public int ContactID { get; set; }
        [Required(ErrorMessage = "This field is required for creating a contact.")]

        [JsonProperty("first_name")]  
        public string First_Name { get; set; }
        [Required(ErrorMessage = "This field is required for creating a contact.")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "This field is required for creating a contact.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required for creating a contact.")]
        public string Phone { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
