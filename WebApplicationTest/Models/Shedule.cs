using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    /// <summary>
    /// Расписание занятий.
    /// </summary>
    [Table("Shedule")]
    public class Shedule
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SheduleId { get; set; }

        public int? TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public int? ClassId { get; set; }

        public Class Class { get; set; }

        [Required]
        public string NumberObject { get; set; }

    }
}