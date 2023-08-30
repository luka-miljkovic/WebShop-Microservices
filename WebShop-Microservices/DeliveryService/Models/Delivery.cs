using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Models
{
    public class Delivery
    {
        [Key]
        [Column("delivery_id")]
        public int DeliveryId { get; set; }
        [Column("delivery_start_time")]
        public DateTime DeliveryStartTime { get; set; }
        [Column("delivery_end_time")]
        public DateTime DeliveryEndTime { get; set; }
        [Column("delivery_employee")]
        public int EmployeeId { get; set; }
        [Column("delivery_order")]
        public string OrderId { get; set; }
    }
}
