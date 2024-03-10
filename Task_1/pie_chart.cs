using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace YourNamespace.Controllers
{
    public class PieChartController : Controller
    {
        public IActionResult Index()
        {
            var client = new RestClient("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={key}");
            var request = new RestRequest(Method.GET);
            var response = client.Execute<List<Employee>>(request);

          
            var totalTimeWorked = response.Data.Sum(e => e.TotalTimeWorked);

            // Generate pie chart
            using (var image = new Bitmap(200, 200))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.White);
                var colors = new[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange }; // Add more colors if needed
                var startAngle = 0f;
                foreach (var employee in response.Data)
                {
                    var sweepAngle = (float)(employee.TotalTimeWorked / totalTimeWorked * 360);
                    var brush = new SolidBrush(colors.First());
                    graphics.FillPie(brush, 0, 0, 200, 200, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }
                image.Save("pie_chart.png", ImageFormat.Png);
            }

            return View();
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public double TotalTimeWorked { get; set; }
    }
}
