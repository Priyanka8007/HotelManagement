using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(100)]
        public string OwnerName { get; set; }

        [Required]
        [MaxLength(15)]
        [Phone]
        public string MobileNo { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        public string GSTNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string LicenseNumber { get; set; }

        // Default value for new hotels to be active
        public bool IsActive { get; set; } = true;

        public DateTime? RenewDate { get; set; } // Nullable DateTime for Renew Date

        [Required]
        public string UserNamee { get; set; }



    }
}
