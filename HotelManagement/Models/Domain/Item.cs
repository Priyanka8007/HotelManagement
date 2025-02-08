using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Domain
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[MaxLength(100)]
        public string ItemName { get; set; }

        [Required]
        //[MaxLength(50)]
        public string ItemCategory { get; set; }

        [Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }



        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Hotel")]
        public int HotelId { get; set; } // Foreign key to Hotel

        public Hotel Hotel { get; set; } // Navigation property
    }
}
