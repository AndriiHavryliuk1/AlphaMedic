using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Program
    {
        const string addressFile = "Data/address";
        const string smallNameFile = "Data/namesSmall";
        const string emailFaile = "Data/email";
        const string degreeFile = "Data/degree";
        const string phoneFile = "Data/phone";
        const string universityFile = "Data/university";
        const string passwordFile = "Data/password";
        const string dateFile = "Data/date";
        const string employmentDate = "Data/newDate";
        static void Main(string[] args)
        {



            // AddDoctors();

            //  AddChedules();

        
            
            Console.WriteLine();
        }

        public static void AddChedules()
        {
            AlphaMedicContext db = new AlphaMedicContext();
            Random rnd = new Random();
             for(int i=0;i<200;i++)
            {
                Schedule sch = new Schedule();
                var start = rnd.Next(0, 16);
                sch.StartWorkingTime = TimeSpan.FromHours(start);
                sch.FinishWorkingTime = TimeSpan.FromHours(start+8);
                db.Schedules.Add(sch);
              }
            db.SaveChanges();
        }



        public static void  AddDoctors()
        {
            List<string> address = ReadFromFile(addressFile);
            List<string> names = ReadFromFile(smallNameFile);
            List<string> emails = ReadFromFile(emailFaile);
            List<string> passwords = ReadFromFile(passwordFile);
            List<string> degrees = ReadFromFile(degreeFile);
            List<string> phones = ReadFromFile(phoneFile);
            List<string> universitys = ReadFromFile(universityFile);
            List<string> birthDate = ReadFromFile(dateFile);
            List<string> employDate = ReadFromFile(employmentDate);


            AlphaMedicContext db = new AlphaMedicContext();

            var deps = db.Departments.Select(x => x.DepartmentId).ToList();



            Random rnd = new Random();
            List<Doctor> doc = new List<Doctor>();
            for (int i = 0; i < 30; i++)
            {
                doc.Add(new Doctor());

                doc.Last().Name = names[i].Split(' ')[0];
                doc.Last().Surname = names[i].Split(' ')[1];
                doc.Last().Phone = phones[i];
                doc.Last().Gender = GenderType.Male;
                doc.Last().DateOfBirth = Convert.ToDateTime(birthDate[i]);
                doc.Last().Address = address[i];
                doc.Last().Email = emails[i];
                doc.Last().Password = passwords[i];
                doc.Last().Active = true;
                doc.Last().EmploymentDate = Convert.ToDateTime(employDate[i]);
                doc.Last().EmploymentRecordBookNumber = rnd.Next(111111, 999999).ToString();
                doc.Last().DismissalDate = null;
                doc.Last().EmployeeType = EmployeeType.Doctor;
                doc.Last().Degree = degrees[rnd.Next(0, 10)];
                doc.Last().Education = universitys[rnd.Next(0, 34)];
                doc.Last().Schedule = null;
                doc.Last().DepartmentId = deps[rnd.Next(0, deps.Count())];
              
                doc.Last().DoctorType = 0;
                db.Doctors.Add(doc.Last());



            }

            db.SaveChanges();
        }




        public static     List<string>  ReadFromFile(string fileName)
        {
            List<string> result = new List<string>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    result.Add(s);
                }

            }

            return result;
        }
    }
}
