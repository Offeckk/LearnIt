using LearnIt.Data.Context;
using LearnIt.Data.Entities;
using LearnIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnIt.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<LIUser> _userManager;

        public HomeController(ILogger<HomeController> logger, LIDbContext context, UserManager<LIUser> userManager) : base(context)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> MyProfile()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = await _userManager.Users.Include(u => u.Courses)
                .ThenInclude(c => c.Status)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();



            LIUser user = currentUser;


            UserProfileViewModel model = new UserProfileViewModel()
            {
                Name = user.UserName,
                Courses = user.Courses.ToList(),
                Roles = await GetUserRoles(user)
            };



            return View(model);
        }

        private async Task<List<string>> GetUserRoles(LIUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
