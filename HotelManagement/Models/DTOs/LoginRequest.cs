using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Optional roles array (usually handled server-side after authentication)
        //public List<string> Roles { get; set; } = new List<string>();
        public string[] Roles { set; get; }

    }
}
