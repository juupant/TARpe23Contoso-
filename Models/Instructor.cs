using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Contoso_University.Models
{
    public class Instructor
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get { return LastName + ", " + FirstMidName; } }
        public ICollection<CourseAssignment>? CourseAssignments { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Hire on:")]
        public DateTime HireDate { get; set; }
        public OfficeAssignment? OfficeAssignment { get; set; }

        [Display(Name = "Days off")]
        public int? DaysOff { get; set; }
        public string? Email { get; set; }
        public int? PhoneNumber { get; set; }

    }
}
