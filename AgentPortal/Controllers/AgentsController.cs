using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgentPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgentPortal.Controllers
{
    public class AgentsController : Controller
    {
        private readonly AgentsRepository _repository;

        public AgentsController(AgentsRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Agent(string code)
        {
            var agent = _repository.GetAgent(code);
            return View(agent);
        }

        [HttpGet]
        public IActionResult NewAgent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAgent(Agent agent)
        {
            _repository.CreateAgent(agent);
            return Redirect("/home/index");
        }

        [HttpPost]
        public void Delete(string code)
        {
            _repository.SoftDelete(code);
        }
    }
}
