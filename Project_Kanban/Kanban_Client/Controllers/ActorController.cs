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
    public class ActorController : Controller
    {
        readonly HttpClient client = new HttpClient();

        public ActorController()
        {
            client.BaseAddress = new Uri("http://localhost:2165/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Actor
        public JsonResult get_actor()
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
            return Json(new{data = actors}, JsonRequestBehavior.AllowGet);
        }
    }
}