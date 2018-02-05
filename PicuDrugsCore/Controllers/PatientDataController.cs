using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicuDrugsCore.Models.PatientData;

namespace PicuDrugsCore.Controllers
{
    public class PatientDataController : Controller
    {
        [HttpGet]
        public IActionResult DrugList()
        {
            return View();
        }
    }
}