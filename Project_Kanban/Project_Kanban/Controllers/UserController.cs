using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project_Kanban.Models;
using Project_Kanban.Utililty;
using System.Data.Entity;
using Newtonsoft.Json;

namespace Project_Kanban.Controllers
{
    public class UserController : ApiController
    {
        ApplicationDbContext myContext = new ApplicationDbContext();
        public IQueryable<User> GetUsers()
        {
            return myContext.Users;
        }
        [HttpGet]
        public IHttpActionResult GetUser(int Id)
        {
            User users = myContext.Users.Find(Id);
            if(users != null)
            {
                return Ok();
            }
            return NotFound();
        }

        public IHttpActionResult GetUsername(string Username)
        {
            User users = myContext.Users.Find(Username);
            if(users != null)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Login(User user)
        {
            var get = GetUsers();
            if (get != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Username))
                {
                    var slog = myContext.Users.Where(e => e.Username == user.Username).SingleOrDefault();
                    if (slog.Username == user.Username)
                    {
                        var check = myContext.Users.FirstOrDefault(a => a.Id == slog.Id);
                        if (check.Id == slog.Id)
                        {
                            var myPassword = Hashing.validatePassword(user.Password, slog.Password);
                            if (myPassword == true)
                            {   
                                return Content(HttpStatusCode.OK, myPassword);

                            }
                            return NotFound();
                        }
                        return NotFound();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpPut]
        public IHttpActionResult ForgotPassword(User user)
        {
            var put = GetUsers();
            if(put != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Username))
                {
                    var slog = myContext.Users.Where(e => e.Username == user.Username).SingleOrDefault();
                    if (slog.Username == user.Username)
                    {
                        var check = myContext.Users.FirstOrDefault(a => a.Id == slog.Id);
                        if(check.Id == slog.Id)
                        {
                            string pass = "12345";
                            check.Password = Hashing.hashPassword(pass);
                            myContext.Entry(check).State = EntityState.Modified;
                            var result = myContext.SaveChanges();
                            if (result > 0)
                            {
                                return Content(HttpStatusCode.OK, result);
                            }
                            return BadRequest();
                        }
                        return NotFound();
                    }
                    return NotFound();
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
 
