using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    /// <summary>
    /// Информация о преподавателях.
    /// </summary>
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FIOteacher { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        public int Exp { get; set; }

        [Required]
        public int CountHour { get; set; }

        public ICollection<Shedule> Shedules { get; set; }

        public Teacher()
        {
            Shedules = new List<Shedule>();
        }

    }
}