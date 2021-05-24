using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgentPortal.Models;
using AgentPortal.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AgentPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var agentsList = new List<Agent>();

            using (var conn = new SqlConnection("Server=.;Database=AgentPortal;Trusted_Connection=True;"))
            {
                conn.Open();
                var cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM AGENTS"
                };

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var agentCode = reader["AgentCode"].ToString();
                    var agentName = reader["AgentName"].ToString();
                    var workingArea = reader["WorkingArea"].ToString();
                    var commission = Decimal.Parse(reader["Commission"].ToString());
                    var phoneNo = reader["PhoneNo"].ToString();

                    agentsList.Add(new Agent
                    {
                        AgentCode = agentCode,
                        AgentName = agentName,
                        WorkingArea = workingArea,
                        Commission = commission,
                        PhoneNo = phoneNo
                    });
                }
            }

            var homeViewModel = new HomeViewModel
            {
                Agents = agentsList
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
