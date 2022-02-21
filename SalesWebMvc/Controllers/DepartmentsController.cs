using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> listDep = new List<Department>();
            listDep.Add(new Department { Id = 1, Name = "Eletronics" });
            listDep.Add(new Department { Id = 2, Name = "Fashion" });

            return View(listDep);
        }
    }
}
