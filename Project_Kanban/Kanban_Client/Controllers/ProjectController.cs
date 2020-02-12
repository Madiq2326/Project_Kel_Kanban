using Newtonsoft.Json;
using Project_Kanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Kanban_Client.Controllers
{
    public class ProjectController : Controller
    {

        readonly HttpClient client = new HttpClient();

        public ProjectController()
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
            IEnumerable<Project> projects = null;
            var responseTask = client.GetAsync("Project");
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
        
        public JsonResult Create(Project project)
        {
            var myContent = JsonConvert.SerializeObject(project);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var projects = client.PostAsync("Project/", ByteContent).Result;
            return Json(new { data = projects }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int Id)
        {
            var cek = client.GetAsync("Project/" + Id).Result;
            var read = cek.Content.ReadAsAsync<Project>().Result;
            return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int Id, Project project)
        {
            var myContent = JsonConvert.SerializeObject(project);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var roles = client.PutAsync("Project/" + Id, ByteContent).Result;
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int Id)
        {
            var roles = client.DeleteAsync("Project/" + Id).ToString();
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }
    }
}