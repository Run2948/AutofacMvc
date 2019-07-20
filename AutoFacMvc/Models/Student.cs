using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFacMvc.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        public int Age { get; set; }
        public int CreatorId { get; set; }
        [StringLength(20)]
        public string CreatorName { get; set; }
    }
}