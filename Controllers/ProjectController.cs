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
        public IActionResult Index(int page = 1)
        {
            PageResultViewModel<ProjectViewModel> pagedResult = new PageResultViewModel<ProjectViewModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Projects?page={page}&pageSize=15").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                pagedResult = JsonConvert.DeserializeObject<PageResultViewModel<ProjectViewModel>>(data);
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
            return View(pagedResult);
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
                project.Tasks = new List<TaskViewModel>();
                string data = JsonConvert.SerializeObject(project);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                ProjectViewModel project = new ProjectViewModel();
                project.Id = id;
                project.Tasks = new List<TaskViewModel>();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Projects/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    project = JsonConvert.DeserializeObject<ProjectViewModel>(data);
                }

                return View(project);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(ProjectViewModel model)
        {
            try
            {
                model.Id = model.Id;
                model.Tasks = new List<TaskViewModel>();
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + $"/Projects/{model.Id}", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Project details updated.";
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                ProjectViewModel project = new ProjectViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Projects/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    project = JsonConvert.DeserializeObject<ProjectViewModel>(data);
                }

                return View(project);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"/Projects/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Project deleted.";
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

        [HttpGet]
           public IActionResult Search(string name)
           {
               List<ProjectViewModel> projects = new List<ProjectViewModel>();

               HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Projects/{name}").Result;

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
    }
}
