using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Domain
{
    public class HotelItemDetails
    {
        [Key]
       public int Id { set; get; }
        public string HotelName { get; set; }
        public string OwnerName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
