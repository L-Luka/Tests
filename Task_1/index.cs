using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EmployeeTimeTrackingApp.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public async Task<ActionResult> Index()
        {
            List<Employee> employees = await GetEmployees();
            return View(employees);
        }

        private async Task<List<Employee>> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={key}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }
                else
                {
                    // Handle error response
                    Console.WriteLine("Error retrieving employee data: " + response.StatusCode);
                }
            }

            return employees;
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public int TotalTimeWorked { get; set; }
    }
}
