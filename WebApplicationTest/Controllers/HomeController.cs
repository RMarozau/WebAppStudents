using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest.Context;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    public class HomeController : Controller
    {
        static SchoolContext db;
        static DataStruct _dts;
        static int? _Id;

        public HomeController()
        {
            DataStruct dts = new DataStruct()
            {
                Server = "vm-odn-1",
                Db = "TestRR",
                User = "odnadmin",
                Password = "1234567"
            };
            _dts = dts;

            db = new SchoolContext(
            DataDb.InitializeConStr(dts));
        }
        public ActionResult Index()
        {

            return View(GetMainLinq());
        }


        /// <summary>
        /// Метод действия для ввода строки подключения(поменяйте в Global.asx старт с этого метода если хотите начать работу с ввода строки подключения)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Connection()
        {
            DataStruct ds = new DataStruct();
            return View(ds);
        }
        [HttpPost]
        public ActionResult Connection(DataStruct dts)
        {
            if (dts.Server == null || dts.Db == null || dts.User == null || dts.Password == null)
            {
                return HttpNotFound();
            }

            _dts = dts;

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Возвращает форму ввода добавления новго обьекта
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Добавление нового обьекта в базу.
        /// </summary>
        /// <param name="modelAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,ClassId,SheduleId,TeacherId,FIOStudent,BirtdayStud," +
            "BirthdayTech,FIOteacher,Exp,CountHour,CountPeople,Reiting,NumberObject")] ModelAddObject modelAdd)
        {

         
            DateTime dtstud;
            DateTime dtTeach;

            try
            {
                bool bSuccess = DateTime.TryParse(modelAdd.BirtdayStud, out dtstud);
                bool bSuccess2 = DateTime.TryParse(modelAdd.BirthdayTech, out dtTeach);
                if (bSuccess == false )
                {
                    ModelState.AddModelError("BirtdayStud", "Дата введена не верно");
                    

                }
                if ( bSuccess2 == false)
                {
                    
                    ModelState.AddModelError("BirthdayTech", "Дата введена не верно");

                }

                if(modelAdd.FIOStudent == null)
                {
                    ModelState.AddModelError("FIOStudent", "ФИО студента должно быть обязательно заполнено");
                }
                if (modelAdd.FIOteacher == null)
                {
                    ModelState.AddModelError("FIOteacher", "ФИО учителя должно быть обязательно заполнено");
                }
                if (modelAdd.Exp == 0)
                {
                    ModelState.AddModelError("Exp", "Опыт учителя должнен быть обязательно заполнено");
                }
                if (modelAdd.CountHour == 0)
                {
                    ModelState.AddModelError("CountHour", "Количество часов должно быть обязательно заполнено");
                }
                if (modelAdd.CountPeople == 0)
                {
                    ModelState.AddModelError("CountPeople", "Количество человек в классе должно быть обязательно заполнено");
                }
                if (modelAdd.Reiting == 0)
                {
                    ModelState.AddModelError("Reiting", "Рейтинг класса должен быть обязательно заполнен");
                }
                if (modelAdd.NumberObject == "")
                {
                    ModelState.AddModelError("NumberObject", "Номер предмета должен быть обязательно заполнен");
                }

                if (ModelState.IsValid)
                {
                    SchoolContext db = new SchoolContext(DataDb.InitializeConStr(_dts));

                    Teacher teacher = new Teacher()
                    {
                        FIOteacher = modelAdd.FIOteacher,
                        Birthday = Convert.ToDateTime(modelAdd.BirthdayTech),
                        Exp = modelAdd.Exp,
                        CountHour = modelAdd.CountHour
                    };

                    db.Teachers.Add(teacher);

                    db.SaveChanges();

                    Class classadd = new Class()
                    {
                        CountPeople = modelAdd.CountPeople,
                        Reiting = modelAdd.Reiting
                    };

                    db.Classes.Add(classadd);

                    db.SaveChanges();

                    Student studadd = new Student()
                    {
                        FIO = modelAdd.FIOStudent,
                        Birtday = Convert.ToDateTime(modelAdd.BirtdayStud),
                        ClassId = classadd.ClassId/*db.Classes.FirstOrDefault(c => c.CountPeople == modelAdd.CountPeople).ClassId*/,

                    };

                    db.Students.Add(studadd);

                    db.SaveChanges();

                    Shedule sheduleadd = new Shedule()
                    {
                        TeacherId = teacher.Id/*db.Teachers.FirstOrDefault(t => t.FIOteacher == modelAdd.FIOteacher).Id*/,
                        ClassId = classadd.ClassId/*db.Classes.FirstOrDefault(t => t.CountPeople == modelAdd.CountPeople).ClassId*/,
                        NumberObject = modelAdd.NumberObject
                    };

                    db.Shedules.Add(sheduleadd);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(modelAdd);
                }
                

            }
            catch
            {
                return HttpNotFound();
            }
            
        }

        /// <summary>
        /// Получает обьект по id и возвращает форму редактирования.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            _Id = id;
            ModelAddObject modelList = GetMainLinq(id);

            if (modelList != null)
            {
                return View(modelList);
            }
            else
            {
                return HttpNotFound();
            }

            
        }

        /// <summary>
        /// Обновление обьекта в базе.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,ClassId,SheduleId,TeacherId,FIOStudent,BirtdayStud," +
            "BirthdayTech,FIOteacher,Exp,CountHour,CountPeople,Reiting,NumberObject")] ModelAddObject model)
        {
            try
            {

                DateTime dtstud;
                DateTime dtTeach;

                bool bSuccess = DateTime.TryParse(model.BirtdayStud, out dtstud);
                bool bSuccess2 = DateTime.TryParse(model.BirthdayTech, out dtTeach);
                if (bSuccess == false)
                {
                    ModelState.AddModelError("BirtdayStud", "Дата введена не верно");


                }
                if (bSuccess2 == false)
                {

                    ModelState.AddModelError("BirthdayTech", "Дата введена не верно");

                }

                if (model.FIOStudent == null)
                {
                    ModelState.AddModelError("FIOStudent", "ФИО студента должно быть обязательно заполнено");
                }
                if (model.FIOteacher == null)
                {
                    ModelState.AddModelError("FIOteacher", "ФИО учителя должно быть обязательно заполнено");
                }
                if (model.Exp == 0)
                {
                    ModelState.AddModelError("Exp", "Опыт учителя должнен быть обязательно заполнено");
                }
                if (model.CountHour == 0)
                {
                    ModelState.AddModelError("CountHour", "Количество часов должно быть обязательно заполнено");
                }
                if (model.CountPeople == 0)
                {
                    ModelState.AddModelError("CountPeople", "Количество человек в классе должно быть обязательно заполнено");
                }
                if (model.Reiting == 0)
                {
                    ModelState.AddModelError("Reiting", "Рейтинг класса должен быть обязательно заполнен");
                }
                if (model.NumberObject == "")
                {
                    ModelState.AddModelError("NumberObject", "Номер предмета должен быть обязательно заполнен");
                }

                if (ModelState.IsValid)
                {

                    SchoolContext db = new SchoolContext(DataDb.InitializeConStr(_dts));

                    Teacher teacher = db.Teachers.FirstOrDefault(t => t.Id == model.TeacherId);

                    teacher.FIOteacher = model.FIOteacher;
                    teacher.Birthday = Convert.ToDateTime(model.BirthdayTech);
                    teacher.Exp = model.Exp;
                    teacher.CountHour = model.CountHour;

                    db.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    Class class1 = db.Classes.FirstOrDefault(t => t.ClassId == model.ClassId);

                    class1.CountPeople = model.CountPeople;
                    class1.Reiting = model.Reiting;

                    db.Entry(class1).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    Student student = db.Students.FirstOrDefault(s => s.Id == model.Id);

                    student.FIO = model.FIOStudent;
                    student.Birtday = Convert.ToDateTime(model.BirtdayStud);

                    db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    Shedule shedule = db.Shedules.FirstOrDefault(s => s.SheduleId == model.SheduleId);
                    shedule.NumberObject = model.NumberObject;


                    db.Entry(shedule).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return HttpNotFound();
            }
           
        }

        /// <summary>
        /// Удаление обьекта по id из базы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                SchoolContext db = new SchoolContext(DataDb.InitializeConStr(_dts));

                if (id == null)
                {
                    return HttpNotFound();
                }
                ModelAddObject model = GetMainLinq(id);

                if (model == null)
                {
                    return HttpNotFound();
                }

                Shedule shedule = db.Shedules.FirstOrDefault(s => s.SheduleId == model.SheduleId);

                db.Shedules.Remove(shedule);

                db.SaveChanges();

                Student student = db.Students.FirstOrDefault(s => s.Id == model.Id);

                db.Students.Remove(student);

                db.SaveChanges();

                Teacher teacher = db.Teachers.FirstOrDefault(t => t.Id == model.TeacherId);

                db.Teachers.Remove(teacher);

                db.SaveChanges();

                Class class1 = db.Classes.FirstOrDefault(t => t.ClassId == model.ClassId);

                db.Classes.Remove(class1);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return HttpNotFound();
            }
        }
        

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Метод полчает список студентов и преподавателей(не используеться).
        /// </summary>
        /// <returns></returns>
        private DataSet GetMainDS()
        {


            SqlConnection connection = new SqlConnection(DataDb.InitializeConStr(_dts));
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(DataDb.SqlMain(), connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            connection.Close();

            return ds;

        }

        /// <summary>
        /// Получение таблицы для вывода на страницу.
        /// </summary>
        /// <returns></returns>
        public List<ModelAddObject> GetMainLinq()
        {
            SchoolContext db = new SchoolContext(
            DataDb.InitializeConStr(_dts));

            var result = (from s in db.Students
                         join Sh in db.Shedules on s.ClassId equals Sh.ClassId
                         join t in db.Teachers on Sh.TeacherId equals t.Id
                         join cl in db.Classes on Sh.ClassId equals cl.ClassId
                         select new
                         {
                             id = s.Id,
                             ClassId = cl.ClassId,
                             SheduleId = Sh.SheduleId,
                             TeacherId = t.Id,
                             FIOStud = s.FIO,
                             BirthdayStud = s.Birtday,
                             FIOteacher = t.FIOteacher,
                             BirthdayTeach = t.Birthday,
                             Exp = t.Exp,
                             CountHour = t.CountHour,
                             CountPeople = cl.CountPeople,
                             NumberObject = Sh.NumberObject,
                             Reiting = cl.Reiting

                         }).ToList();

            List<ModelAddObject> model = new List<ModelAddObject>();

            foreach(var i in result)
            {
                model.Add(new ModelAddObject()
                {
                    Id = i.id,
                    ClassId = i.ClassId,
                    SheduleId = i.SheduleId,
                    TeacherId = i.TeacherId,
                    FIOStudent = i.FIOStud,
                    BirtdayStud = i.BirthdayStud.ToShortDateString(),
                    FIOteacher = i.FIOteacher,
                    BirthdayTech = i.BirthdayTeach.ToShortDateString(),
                    Exp = i.Exp,
                    CountHour = i.CountHour,
                    CountPeople = i.CountPeople,
                    NumberObject = i.NumberObject,
                    Reiting = i.Reiting

                });
            }

            return model;
        }
        /// <summary>
        /// получение обьекта по id/
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModelAddObject GetMainLinq(int? id)
        {
            SchoolContext db = new SchoolContext(
            DataDb.InitializeConStr(_dts));

            var result = (from s in db.Students
                          join Sh in db.Shedules on s.ClassId equals Sh.ClassId
                          join t in db.Teachers on Sh.TeacherId equals t.Id
                          join cl in db.Classes on Sh.ClassId equals cl.ClassId
                          where s.Id == id
                          select new
                          {
                              id = s.Id,
                              ClassId = cl.ClassId,
                              SheduleId = Sh.SheduleId,
                              TeacherId = t.Id,
                              FIOStud = s.FIO,
                              BirthdayStud = s.Birtday,
                              FIOteacher = t.FIOteacher,
                              BirthdayTeach = t.Birthday,
                              Exp = t.Exp,
                              CountHour = t.CountHour,
                              CountPeople = cl.CountPeople,
                              NumberObject = Sh.NumberObject,
                              Reiting = cl.Reiting

                          }).ToList();

            ModelAddObject model = new ModelAddObject();

            foreach (var i in result)
            {
                model = new ModelAddObject()
                {
                    Id = i.id,
                    ClassId = i.ClassId,
                    SheduleId = i.SheduleId,
                    TeacherId = i.TeacherId,
                    FIOStudent = i.FIOStud,
                    BirtdayStud = i.BirthdayStud.ToShortDateString(),
                    FIOteacher = i.FIOteacher,
                    BirthdayTech = i.BirthdayTeach.ToShortDateString(),
                    Exp = i.Exp,
                    CountHour = i.CountHour,
                    CountPeople = i.CountPeople,
                    NumberObject = i.NumberObject,
                    Reiting = i.Reiting

                };
            }

            return model;
        }
        /// <summary>
        /// Получает список по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private DataSet GetMainDS(int? id)
        {


            SqlConnection connection = new SqlConnection(DataDb.InitializeConStr(_dts));
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(DataDb.SqlMain(id), connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            connection.Close();

            return ds;

        }
    }
}