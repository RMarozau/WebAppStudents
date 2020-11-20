using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    /// <summary>
    /// Модель класса в учебной заведении.
    /// </summary>
    [Table("Class")]
    public class Class
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }

        [Required]
        public int CountPeople { get; set; }

        [Required]
        public int Reiting { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Shedule> Shedules { get; set; }


        public Class()
        {
            Students = new List<Student>();
            Shedules = new List<Shedule>();
        }

       

        

    }
}