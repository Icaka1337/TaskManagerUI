using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TaskManagerUI.Models;

namespace TaskManagerUI.Controllers
{
    public class TaskController : Controller
    {
        private readonly Uri _baseAddress = new Uri("https://localhost:7293/api");
        private readonly HttpClient _client;
        private string JwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QyIiwibmFtZWlkIjoiMjAwMyIsIm5iZiI6MTcxNjgzMjI1MywiZXhwIjoxNzE2ODc1NDUzLCJpYXQiOjE3MTY4MzIyNTN9.pe9dFVwolOIQiE3nI7A-urOF78UU8gl7ejE5vvENyWE";

        public TaskController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
        }

        public IActionResult Index()
        {
            List<TaskViewModel> tasks = new List<TaskViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Tasks").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                tasks = JsonConvert.DeserializeObject<List<TaskViewModel>>(data);
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskViewModel task)
        {
            try
            {
                if (task == null)
                {
                    return View("Error");
                }
                string data = JsonConvert.SerializeObject(task);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage reponse = _client.PostAsync(_client.BaseAddress + "/Tasks/PostTask", content).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Task Created.";
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
                TaskViewModel task = new TaskViewModel();
                task.Id = id;
                task.UserTasks = new List<UserTaskViewModel>();
                task.Project= new ProjectViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Tasks/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    task = JsonConvert.DeserializeObject<TaskViewModel>(data);
                }

                return View(task);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + $"/Tasks/{model.Id}", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Task details updated.";
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
                TaskViewModel task = new TaskViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Tasks/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    task = JsonConvert.DeserializeObject<TaskViewModel>(data);
                }

                return View(task);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"/Tasks/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Task deleted.";
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
