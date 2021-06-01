using LearnIt.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LearnIt.Controllers
{
    public class BaseController : Controller
    {
        public readonly LIDbContext _context;

        public BaseController(LIDbContext context)
        {
            _context = context;
            //UpdateLearnCourseStatus();
        }
    }
}
