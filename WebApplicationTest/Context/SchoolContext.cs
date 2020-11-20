using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using WebApplicationTest.Models;

namespace WebApplicationTest.Context
{
    public class SchoolContext:DbContext
    {
        public SchoolContext(string connectionString)
            :base(connectionString)
        {
            
        }
        public SchoolContext()
        { 

        }

        static SchoolContext()
        {          
          Database.SetInitializer(new SetInitiliazer());
           
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Shedule> Shedules { get; set; } 

        public DbSet<Class> Classes { get; set; }




    }

    public class SetInitiliazer : DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            Teacher[] teachersDefault = new Teacher[]
            {
                new Teacher() { FIOteacher = "Киано Ривз", Birthday = new DateTime(1975,01,01), Exp = 12, CountHour = 75 },
                new Teacher() { FIOteacher = "Аль Пачино", Birthday = new DateTime(1945,05,03), Exp = 34, CountHour = 34 },
                new Teacher() { FIOteacher = "Джосеф Гордон Левит", Birthday = new DateTime(1989,10,19), Exp = 54, CountHour = 23 },
                new Teacher() { FIOteacher = "Моника Белучи", Birthday = new DateTime(1964,07,12), Exp = 14, CountHour = 54 }
            };

            foreach(var t in teachersDefault)
            {
                context.Teachers.Add(t);
            }
            context.SaveChanges();

            Class[] classDefault = new Class[]
            {
                new Class() { CountPeople = 30, Reiting = 5},
                new Class() { CountPeople = 25, Reiting = 3},
                new Class() { CountPeople = 27, Reiting = 4},
                new Class() { CountPeople = 24, Reiting = 5},
            };

            foreach (var c in classDefault)
            {
                context.Classes.Add(c);
            }

            context.SaveChanges();

            Student[] studentDefault = new Student[]
            {
                new Student()
                {   FIO = "Дэниел Рэдклиф",
                    Birtday = new DateTime(1995,12,26),
                    ClassId =  context.Classes.FirstOrDefault(c=>c.CountPeople == 30).ClassId,

                },
                new Student()
                {   FIO = "Юсеин Болт",
                    Birtday = new DateTime(1996,11,23),
                    ClassId =  context.Classes.FirstOrDefault(c=>c.CountPeople == 25).ClassId,

                },
                 new Student()
                {   FIO = "Рон Уизли",
                    Birtday = new DateTime(2000,02,12),
                    ClassId =   context.Classes.FirstOrDefault(c=>c.CountPeople == 27).ClassId,

                },
                  new Student()
                {   FIO = "Дженифер Энистон",
                    Birtday = new DateTime(1994,04,14),
                    ClassId = context.Classes.FirstOrDefault(c=>c.CountPeople == 24).ClassId,

                },
            };

            foreach(var i in studentDefault)
            {
                context.Students.Add(i);
            }

            context.SaveChanges();

            Shedule[] shedulesDefault = new Shedule[]
            {
                new Shedule()
                {
                  TeacherId = context.Teachers.FirstOrDefault(t=>t.FIOteacher == "Киано Ривз").Id,
                  ClassId = context.Classes.FirstOrDefault(t=>t.CountPeople == 30).ClassId,
                  NumberObject = "1"
                },
                 new Shedule()
                {
                  TeacherId = context.Teachers.FirstOrDefault(t=>t.FIOteacher == "Аль Пачино").Id,
                  ClassId = context.Classes.FirstOrDefault(t=>t.CountPeople == 24).ClassId,
                  NumberObject = "2"
                },
                 new Shedule()
                {
                  TeacherId = context.Teachers.FirstOrDefault(t=>t.FIOteacher == "Моника Белучи").Id,
                  ClassId = context.Classes.FirstOrDefault(t=>t.CountPeople == 27).ClassId,
                  NumberObject = "3"
                },
            };

            foreach (var i in shedulesDefault)
            {
                context.Shedules.Add(i);
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }

    public static class DataDb
    {
        public static string InitializeConStr(DataStruct dts)
        {
            string connectionString = $"Data Source={dts.Server};" +
                                        $"Initial Catalog={dts.Db};" +
                                        $"User ID={dts.User};" +
                                        $"Password={dts.Password};" +
                                        $"Integrated Security=False";

            return connectionString;
        }

        public static string SqlMain()
        {
            return $"select s.Id, FIO as FIOStud, Birtday as BirthdayStud, FIOteacher, Birthday as BirthdayTeach, Exp, CountHour, CountPeople, Reiting  " +
                $"from dbo.Student as s " +
                $"join Shedule as sh ON sh.ClassId = s.ClassId " +
                $"join Teacher as t ON sh.TeacherId = t.Id " +
                $"join Class as cl ON sh.ClassId = cl.ClassId";
        }

        public static string SqlMain(int? id)
        {
            return $"select s.Id, FIO as FIOStud, Birtday as BirthdayStud, FIOteacher, Birthday as BirthdayTeach, Exp, CountHour, CountPeople, Reiting, NumberObject  " +
                $"from dbo.Student as s " +
                $"join Shedule as sh ON sh.ClassId = s.ClassId " +
                $"join Teacher as t ON sh.TeacherId = t.Id " +
                $"join Class as cl ON sh.ClassId = cl.ClassId " +
                $"where s.Id = {id}";
        }

    }

    public struct DataStruct
    {
        public string Server { get; set; }
        public string Db { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}