using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public string[] Roles { set; get; }
        // Use a single Role string instead of a list
        public string[] Roles { set; get; }
    }
}
