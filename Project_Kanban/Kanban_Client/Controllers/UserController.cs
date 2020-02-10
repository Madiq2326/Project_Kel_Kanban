using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project_Kanban.Models;
using Project_Kanban.Utililty;

namespace Kanban_Client.Controllers
{
    public class UserController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public UserController()
        {
            client.BaseAddress = new Uri("http://localhost:2165/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult LoginView()
        {
            return View(GetDataUsers());
        }
        public JsonResult GetDataUsers()
        {
            IEnumerable<User> users = null;
            var responseTask = client.GetAsync("User");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<User>>();
                readTask.Wait();
                users = readTask.Result;
            }
            else
            {
                users = Enumerable.Empty<User>();
                ModelState.AddModelError(String.Empty, "404 Not Found");
            }
            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Login(User users)
        {
            var myContent = JsonConvert.SerializeObject(users);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var login = client.PostAsync("User",byteContent);
            Session["username"] = users.Username;
            Session.Add("username", users.Username);
            return Json(new { data = login }, JsonRequestBehavior.AllowGet);            
        }

        //public JsonResult Details(string Username)
        //{
        //    var responseTask = client.GetAsync("User/" + Username).Result;
        //    var read = responseTask.Content.ReadAsAsync<User>().Result;
        //    return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult ForgotPassword(User users)
        {
            var myContent = JsonConvert.SerializeObject(users);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var fPassword = client.PutAsync("User",byteContent).Result;
            MailMessage sMail = new MailMessage();
            sMail.To.Add(new MailAddress(users.Username));
            sMail.From = new MailAddress("cobamvc@gmail.com");
            sMail.Subject = "[Password] " + DateTime.Now.ToString("dd/mm/yyyy/hh:mm:ss");
            sMail.Body = "Hello "+ users.Username+"\nThis Is Your New Password : 12345";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("cobamvc@gmail.com", "@Bootcamp33");
            smtp.Send(sMail);
            Console.WriteLine("Password Has Been Sent To Your Reserved Email. Please Kindly To Check Your Email To Login");
            return Json(new { data = fPassword }, JsonRequestBehavior.AllowGet);
        }
    }
}