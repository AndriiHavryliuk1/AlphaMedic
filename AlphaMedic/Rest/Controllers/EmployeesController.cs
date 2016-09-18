using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System.Web.Routing;
using System.Data.Entity.Validation;
using Rest.Dtos;

namespace Rest.Controllers
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        // GET: api/Employees
        [Route("")]
        [Authorize(Roles = Roles.Administrator)]
        public IHttpActionResult GetEmployees()
        {
            var res = db.Employees.Select(x => new
            {
                x.UserId,
                x.Name,
                x.Surname,
                x.DateOfBirth,
                x.Phone,
                x.Email,
                x.Address,
                x.EmploymentRecordBookNumber,
                EmployeeType = x.EmployeeType.ToString(),
                x.Active
            });

            return Ok(res);
        }

        // GET: api/Employees/5
        [Route("{id:int}")]
        [Authorize(Roles=Roles.Administrator)]
        public IHttpActionResult GetEmployee(int id)
        {
            var employee = db.Employees.FirstOrDefault(x => x.UserId == id);
            if (employee == null)
            {
                return NotFound();
            }

            var res = new
            {
                employee.UserId,
                employee.Name,
                employee.Surname,
                employee.EmployeeType,
                employee.Active,
                URLImage = Constants.ThisServer + employee.URLImage
            };

            return Ok(res);
        }



        // PUT: api/Employees/5
        [Route("{id:int}")]
        [Authorize(Roles = Roles.Administrator)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, EmployeeDto employeeDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             if (id != employeeDto.UserId)
            {
                return BadRequest();
            }
         
            var emp=db.Employees.Find(employeeDto.UserId);

            var elm = db.Users.FirstOrDefault(x => x.Email == employeeDto.Email);
            if (elm != null && elm.UserId != id)
            {
                return BadRequest("Email is allready exists");
            }

            employeeDto.UpateEmployee(emp);

            db.Entry(emp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        // POST: api/Employees
        [Authorize(Roles = Roles.Administrator)]
        [Route("administrators")]
        [ResponseType(typeof(Administrator))]
        public IHttpActionResult PostAdministrator(Administrator employee)
        {
            if (!ModelState.IsValid || (db.Users.FirstOrDefault(x => x.Email == employee.Email) != null))
            {
                return BadRequest(ModelState);
            }

            try
            {
                FillEmploye(employee);

                db.Users.Add(employee);

                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        // POST: api/Employees
        [Authorize(Roles =Roles.Administrator)]
        [Route("receptionists")]      
        [ResponseType(typeof(Receptionist))]
        public IHttpActionResult PostReceptionist(Receptionist employee)
        {
            if (!ModelState.IsValid|| (db.Users.FirstOrDefault(x => x.Email == employee.Email) != null))
            {
                return BadRequest(ModelState);
            }

            try
            {
                FillEmploye(employee);

                db.Users.Add(employee);

                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }            
        }

       
        private void FillEmploye(Employee employee)
        {
            employee.UserClaim = db.UserClaims.FirstOrDefault(x => x.ClaimValue == employee.EmployeeType.ToString());
            employee.EmploymentDate = DateTime.Now;
            employee.URLImage = Constants.DefaultDoctorImage;
        }

    /*    // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Users.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}