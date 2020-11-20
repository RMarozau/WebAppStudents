using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTest.Models
{
    /// <summary>
    /// Модель для вывода на главную страницу информации о студентах и преподавателях.
    /// </summary>
    public class ModelAddObject
    {
        public int Id { get; set; }

        public int ClassId { get; set; }

        public int SheduleId { get; set; }

        public int TeacherId { get; set; }
        public string FIOStudent { get; set; }
        public string BirtdayStud { get; set; }

        public string FIOteacher { get; set; }

        public string BirthdayTech { get; set; }

        public int Exp { get; set; }
        public int CountHour { get; set; }

        public int CountPeople { get; set; }

        public int Reiting { get; set; }

        public string NumberObject { get; set; }

    }
}

