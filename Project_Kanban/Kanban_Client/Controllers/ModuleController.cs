using Newtonsoft.Json;
using Project_Kanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Kanban_Client.Controllers
{
    public class ModuleController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        readonly HttpClient client = new HttpClient();

        public ModuleController()
        {
            client.BaseAddress = new Uri("http://localhost:2165/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            IEnumerable<Module> modules = null;
            var responseTask = client.GetAsync("Module/GetModules");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Module>>();
                readTask.Wait();
                modules = readTask.Result;
            }
            else
            {
                modules = Enumerable.Empty<Module>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(new { data = modules }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(Module module)
        {
            var myContent = JsonConvert.SerializeObject(module);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var modules = client.PostAsync("Module/", ByteContent).Result;
            return Json(new { data = modules }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int Id)
        {
            var cek = client.GetAsync("Module/" + Id).Result;
            var read = cek.Content.ReadAsAsync<Module>().Result;
            return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int Id, Module module)
        {
            var get_email = con.Query<Actor>("SELECT Email FROM TB_M_Actor WHERE Id = @Actor_Id",new { Actor_Id = module.Actor_Id }).SingleOrDefault();

            if (module.Status == "Testing")
            {
                MailMessage mm = new MailMessage("Madiq2326@gmail.com", get_email.Email);
                mm.Subject = "Tesing Application";
                mm.Body = "Module " + module.Name + " is ready for testing ";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("Madiq2326@gmail.com", "Mind2326");
                smtp.Send(mm);
            }

            var myContent = JsonConvert.SerializeObject(module);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var modules = client.PutAsync("Module/" + Id, ByteContent).Result;
            return Json(new { data = modules }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int Id)
        {
            var modules = client.DeleteAsync("Module/" + Id).ToString();
            return Json(new { data = modules }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Listbyproject(int Id)
        {
            var read = con.Query<Module>("SELECT * FROM TB_T_Module WHERE @project = Project_Id",
               new { project = Id }).ToList();
            return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List_Project()
        {
            IEnumerable<Project> projects = null;
            var responseTask = client.GetAsync("Project/GetProjects");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Project>>();
                readTask.Wait();
                projects = readTask.Result;
            }
            else
            {
                projects = Enumerable.Empty<Project>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(new { data = projects }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult view_project()
        {
            IEnumerable<Project> projects = null;
            var responseTask = client.GetAsync("Project");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<Project>>();
                readTask.Wait();
                projects = readTask.Result;
            }
            else
            {
                projects = Enumerable.Empty<Project>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json( projects , JsonRequestBehavior.AllowGet);
        }

        public ActionResult view_actor()
        {
            IEnumerable<Actor> actors = null;
            var responseTask = client.GetAsync("Actor");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<Actor>>();
                readTask.Wait();
                actors = readTask.Result;
            }
            else
            {
                actors = Enumerable.Empty<Actor>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json( actors , JsonRequestBehavior.AllowGet);
        }

    }
}