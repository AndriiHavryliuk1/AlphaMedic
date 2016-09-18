using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RestWebApi.Models;
using RestWebApi.DTOs;

namespace RestWebApi.Controllers
{
    [RoutePrefix("api/departments")]
    public class DepartmentsController : ApiController
    {
        private RestWebApiContext db = new RestWebApiContext();

        //private static readonly Expression<Func<Department, DepartmentDto>> AsDepartmentDto =
        //    x => new DepartmentDto
        //    {
        //        DepartmentId = x.DepartmentId.ToString(),
        //        DepartmentName = x.DepartmentName,
        //        DepartmentDescription = x.DepartmentDescription
        //    };

        // GET: api/Departments
        [Route("")]
        public IQueryable GetDepartments()
        {
            return from d in db.Departments
                   select new
                   {
                       DepartmentId = d.DepartmentId,
                       DepartmentName = d.DepartmentName,
                       DepartmentDescription = d.DepartmentDescription
                   };
        }

        // GET: api/Departments/5
        [Route("{id:int}")]
        //[ResponseType(typeof(DepartmentDetailsDto))]
        public IHttpActionResult GetDepartment(int id)
        {
            // DepartmentDetailsDto department = DtoHelper.GetDepartmentDetailsDto(db.Departments.Find(id));

            var department = (from d in db.Departments
                              where d.DepartmentId == id
                              join v in db.Doctors on d.DepartmentHeadId equals v.DoctorId
                              join u in db.Users on v.UserId equals u.UserId
                              //select d;
                              select new
                              {
                                  d.DepartmentId,
                                  d.DepartmentName,
                                  d.DepartmentDescription,
                                  v.DoctorId,
                                  DoctorFullName = u.UserName + " " + u.UserSurname,
                                  Feedbacks = from f in db.Feedbacks
                                              join u2 in db.Users on f.UserId equals u2.UserId
                                              where f.DepartmentId == d.DepartmentId
                                              select new
                                              {
                                                  f.FeedbackId,
                                                  f.FeedbackDescription,
                                                  u2.UserId,
                                                  UserFullName = u2.UserName + " " + u2.UserSurname,
                                                  f.FeedbackDate
                                              }
                              }).SingleOrDefault();




            //var department = (from d in db.Departments
            //                  where d.DepartmentId == id
            //                  select new
            //                  {
            //                      DepartmentId = d.DepartmentId,
            //                      DepartmentName = d.DepartmentName,
            //                      DepartmentDescription = d.DepartmentDescription,
            //                      DepartmentHead = (from u in db.Doctors
            //                                        from v in db.Users
            //                                        where u.DoctorId == d.DepartmentHeadId &&
            //                                        v.UserId == u.UserId
            //                                        select new
            //                                        {
            //                                            UserId = v.UserId,
            //                                            UserName = v.UserName,
            //                                            UserSurname = v.UserSurname
            //                                        }).FirstOrDefault(),
            //                      Feedbacks = from f in db.Feedbacks
            //                                  where f.DepartmentId == d.DepartmentId
            //                                  select new
            //                                  {
            //                                      FeedbackId = f.FeedbackId,
            //                                      FeedbackDescription = f.FeedbackDescription,
            //                                      User = (from u in db.Users
            //                                              where f.UserId == u.UserId
            //                                              select new
            //                                              {
            //                                                  UserId = u.UserId,
            //                                                  UserName = u.UserName,
            //                                                  UserSurname = u.UserSurname
            //                                              }).FirstOrDefault()
            //                                  }
            //                  }).FirstOrDefault();


            //var department = db.Departments.Find(id, DtoHelper.AsDepartmentDetailsDto);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Departments
        [ResponseType(typeof(Department))]
        public IHttpActionResult PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}