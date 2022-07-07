using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MegaTravelClient.Models
{
    public class RegistrationModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Street1 { get; set; } = null!;
        public string Street2 { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [HiddenInput]
        public string UserType { get; set; } = null!;
    }
}
