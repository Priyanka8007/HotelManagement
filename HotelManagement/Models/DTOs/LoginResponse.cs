using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DTOs
{
    public class LoginResponse
    {
        public string JwtToken { get; set; }

    }
}
