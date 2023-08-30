using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("available")]
        public bool Available { get; set; }
    }
}
