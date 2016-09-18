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
//using JsonPatch;
using System.Data.Entity.Validation;
using Microsoft.AspNetCore.JsonPatch;
using Rest.Dtos;
using Rest.Helpers;

namespace Rest.Controllers
{   [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
               return db.Users;
        }
        // GET: api/Users/5
        [Authorize]
        [Route("{id:int}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            var res = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Surname = user.Surname,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Phone = user.Phone,
                Address = user.Address,
                URLImage = Constants.ThisServer + user.URLImage
            };
            return Ok(res);
        }
        [Authorize]
        [Route("{id:int}")]
        public IHttpActionResult PatchActiveState(int id, JsonPatchDocument<User> patchData)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if (!this.User.IsInRole(Roles.Administrator) &&  currentUser.UserId!=id )
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            var objectToUpdate = db.Users.Find(id);
            patchData.ApplyTo(objectToUpdate);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string s = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                         eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return BadRequest(s);
            }
            return Ok();

        }



        // PUT: api/Users/5
        // PUT: api/Patients/5
        [Authorize]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("{id:int}", Name = "PutUser")]
        public IHttpActionResult PutUser(int id, UserDto user)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if (!this.User.IsInRole(Roles.Administrator) && currentUser.UserId != id)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }
            
            var tmp = db.Users.Find(id);

            if (tmp == null)
            {
                return NotFound();
            }

            tmp.Name = user.Name;
            tmp.Surname = user.Surname;
            tmp.Phone = user.Phone;
            tmp.DateOfBirth = user.DateOfBirth;
            tmp.Address = user.Address;
            tmp.Gender = user.Gender;


            db.Entry(tmp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        [HttpPost]
        [Route("recovery")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostRecoveryPassword([FromUri]string email)
        {

            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            var newpass = System.Web.Security.Membership.GeneratePassword(6, 0);


            EmailInput emailInput = new EmailInput();
            //var pat = db.Patients.Find(emailPostDto.appointment.PatientId);
            emailInput.UserName = user.Name + " " + user.Surname;
            emailInput.Email = user.Email;
            emailInput.Subject = "Recovery password!";

            emailInput.Body = 
                "Hello! \nWe have recodered your password\nYour new password: " + newpass + 
                "\nPlease change it when you login to the system" + 
                "\nBest regard,\nAlphaMedic" ;
            try
            {
                EMailHelper.SendNotification(emailInput);
                user.Password = HashHelper.sha256_hash(newpass);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);

        }



        [Authorize]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("changepass/{id:int}", Name = "ChangePass")]
        public IHttpActionResult PutUser(int id, ChangePass user)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email==this.User.Identity.Name);

            if(currentUser.UserId != id)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            var tmp = db.Users.Find(id);

            if(tmp == null)
            {
                return NotFound();
            }

            if (user.OldPass != tmp.Password)
            {
                return BadRequest();
            }

            tmp.Password = user.NewPass;

            db.Entry(tmp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}