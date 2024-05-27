using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TaskManagerUI.Models;


namespace TaskManagerUI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Uri _baseAddress = new Uri("https://localhost:7293/api");
        private readonly HttpClient _client;
        private string JwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QyIiwibmFtZWlkIjoiMjAwMyIsIm5iZiI6MTcxNjgzMjI1MywiZXhwIjoxNzE2ODc1NDUzLCJpYXQiOjE3MTY4MzIyNTN9.pe9dFVwolOIQiE3nI7A-urOF78UU8gl7ejE5vvENyWE";

        public ProjectController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Projects").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                projects = JsonConvert.DeserializeObject<List<ProjectViewModel>>(data);
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel project)
        {
            try
            {
                if (project == null)
                {
                    return View("Error");
                }
                project.Tasks= new List<Task>();
                string data = JsonConvert.SerializeObject(project);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Projects/Post").Result;
                HttpResponseMessage reponse = _client.PostAsync(_client.BaseAddress + "/Projects/PostProject", content).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Project Created.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;
                return View();
            }
            return View();
        }
    }
}
