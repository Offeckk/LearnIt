using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearnIt.Data.Context;
using LearnIt.Data.Entities;
using LearnIt.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LearnIt.Controllers
{
    [Authorize]
    public class LearnCoursesController : BaseController
    {
        private readonly UserManager<LIUser> _userManager;
        public LearnCoursesController(LIDbContext context, UserManager<LIUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {


            string currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser currentUser = await _context.Users.FindAsync(currentUserId);



            var courses = await _context.LearnCourses.Include(l => l.Status).Include(u => u.Users).ToListAsync();
            courses = await UpdateLearnCourseStatus(courses);

            if (await _userManager.IsInRoleAsync(currentUser, "Teacher") && !(await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                List<LearnCourse> teacherCourses = courses.Where(c => c.Users.Contains(currentUser)).ToList();
                List<LearnCourse> foreignCourses = courses.Except(teacherCourses).ToList();


                TeacherIndexViewModel teacherIndexViewModel = new TeacherIndexViewModel()
                {
                    TeacherCourses = teacherCourses,
                    ForeignCourses = foreignCourses
                };

                return View("TeacherIndex", teacherIndexViewModel);
            }

            if (await _userManager.IsInRoleAsync(currentUser, "Student") && !(await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                courses.RemoveAll(c => c.StatusId == "3" || c.StatusId == "4");
            }

            return View(courses);
        }

        [Authorize(Roles = "Admin,Student,Teacher")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser currentUser = await _context.Users.FindAsync(currentUserId);
            
            var learnCourse = await _context.LearnCourses
                .Include(l => l.Status)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            var teachers = _userManager.GetUsersInRoleAsync("Teacher").Result;           

            var students = await _userManager.Users.Include(c => c.Courses)
                .Where(c => c.Courses.Contains(learnCourse))
                .Where(i => !teachers.Select(t => t.Id).Contains(i.Id))
                .ToListAsync();

            var studentsName = students.Select(s => s.UserName).ToList();
         
            var teacher = _context.Users.Include(c => c.Courses)
                .Where(c => c.Courses.Contains(learnCourse))
                .FirstOrDefault().UserName;

            LearnCourseDetailsViewModel model = new LearnCourseDetailsViewModel()
            {
                Id = learnCourse.Id,
                Name = learnCourse.Name,
                Description = learnCourse.Description,
                StartDate = learnCourse.StartDate,
                EndDate = learnCourse.EndDate,
                Status = learnCourse.Status.Name,
                Students = studentsName,
                Teacher = teacher
            };

            if (learnCourse.Users != null && learnCourse.Users.Contains(currentUser))
            {
                model.IsCurrentUserJoined = true;
            }

            if (learnCourse == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {      
            return View();
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LearnCourseCreateViewModel model)
        {
            string teacherId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser teacher = await _context.Users.FindAsync(teacherId);

            if (ModelState.IsValid)
            {
                var courseStatus = await _context.CourseStatuses.FirstOrDefaultAsync(x => x.Id == "1");

                var learnCourse = new LearnCourse()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = courseStatus,
                    StatusId = courseStatus.Id,
                    Users = new List<LIUser>()

                };
                learnCourse.Users.Add(teacher);

                _context.Add(learnCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }           
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id)
        {
            string currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser currentUser = await _context.Users.FindAsync(currentUserId);

            

            if (id == null)
            {
                return NotFound();
            }

            LearnCourseEditViewModel model = new LearnCourseEditViewModel()
            {
                Id = id,
                Statuses = await this._context.CourseStatuses.ToListAsync()
            };

            var learnCourse = await _context.LearnCourses.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == id);
            if (learnCourse == null)
            {
                return NotFound();
            }

            if (!learnCourse.Users.Contains(currentUser) && !(await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                ErrorPageViewModel errorModel = new ErrorPageViewModel()
                {
                    ErrorMessage = "You are not the teacher of this course"
                };
                return View("ErrorPage", errorModel);
            }

            model.Name = learnCourse.Name;
            model.Description = learnCourse.Description;
            model.StartDate = learnCourse.StartDate;
            model.EndDate = learnCourse.EndDate;
            model.Status = learnCourse.Status.Name;

            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LearnCourseEditViewModel model)
        {
            if (id != model.Id)
            {
               return NotFound();
            }

            var learnCourse = await _context.LearnCourses.FirstOrDefaultAsync(x => x.Id == id);
            var learnStatus = await _context.CourseStatuses.FirstOrDefaultAsync(x => x.Name == model.Status);

            if (ModelState.IsValid)
            {
                try
                {
                    learnCourse.Name = model.Name;
                    learnCourse.Description = model.Description;
                    learnCourse.StartDate = model.StartDate;
                    learnCourse.EndDate = model.EndDate;
                    learnCourse.Status = learnStatus;
                    learnCourse.StatusId = learnStatus.Id;
                    

                    _context.Update(learnCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnCourseExists(learnCourse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            model.Id = id;
            model.Statuses = await this._context.CourseStatuses.ToListAsync();
            
            return View(model);
        }

        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> JoinCourse(string id)
        {


            if (id == null)
            {
                return NotFound();
            }


            var learnCourse = await _context.LearnCourses
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (learnCourse.Status.Name != "Upcoming")
            {
                ErrorPageViewModel errorModel = new ErrorPageViewModel()
                {
                    ErrorMessage = "The course must be in \"Upcomming\" status"
                };
                return View("ErrorPage", errorModel);
            }

            string studentId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser student = await _context.Users.FindAsync(studentId);

            if (learnCourse.Users != null && learnCourse.Users.Select(u => u.Id).Contains(studentId))
            {
                ErrorPageViewModel errorModel = new ErrorPageViewModel()
                {
                    ErrorMessage = "You are already in the course."
                };

                return View("ErrorPage", errorModel);
            }

            if (learnCourse == null)
            {
                return NotFound();
            }

            return View(learnCourse);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPost, ActionName("JoinCourse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinCourseConfirmed(string id)
        {
            LearnCourse learnCourse = await _context.LearnCourses.Include(s => s.Users).FirstOrDefaultAsync(x => x.Id == id);
            string studentId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LIUser student = await _context.Users.FindAsync(studentId);

            if (learnCourse.Status.Name != "Upcoming")
            {
                ErrorPageViewModel errorModel = new ErrorPageViewModel()
                {
                    ErrorMessage = "The course must be in \"Upcomming\" status"
                };
                return View("ErrorPage", errorModel);
            }

            if (learnCourse.Users != null && learnCourse.Users.Select(u => u.Id).Contains(studentId))
            {
                ErrorPageViewModel errorModel = new ErrorPageViewModel()
                {
                    ErrorMessage = "You are already in the course."
                };

                return View("ErrorPage", errorModel);
            }

            learnCourse.Users.Add(student);

            _context.Update(learnCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnCourse = await _context.LearnCourses
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learnCourse == null)
            {
                return NotFound();
            }

            return View(learnCourse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var learnCourse = await _context.LearnCourses.FindAsync(id);
            _context.LearnCourses.Remove(learnCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> KickUserFromCourse(string userName, string courseId)
        {
            

            var learnCourse = await _context.LearnCourses.Include(u => u.Users).Where(c => c.Id == courseId).FirstOrDefaultAsync();

            var student = await _userManager.Users.Include(c => c.Courses)
               .Where(c => c.Courses.Contains(learnCourse))
               .Where(s => s.UserName == userName)
               .FirstOrDefaultAsync();

            LearnCourse course = learnCourse;
            int userRemoveIndex = course.Users.ToList().FindIndex(l => l.UserName == userName);

            LIUser user = student;
            int courseRemoveIndex = user.Courses.ToList().FindIndex(l => l.Id == courseId);



            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (course != null && user != null)
                {
                    course.Users.RemoveAt(userRemoveIndex);
                    user.Courses.RemoveAt(courseRemoveIndex);
                }
               
                
                _context.Update(course);
                await _context.SaveChangesAsync();

                return View("../Home/Index");

            }
        }

        private bool LearnCourseExists(string id)
        {
            return _context.LearnCourses.Any(e => e.Id == id);
        }

        private async Task<List<LearnCourse>> UpdateLearnCourseStatus(List<LearnCourse> courses)
        {
            DateTime now = DateTime.Now;

            var upcomingCourses = courses.Where(x => x.Status.Id == "1").ToList();
            var activeCourses = courses.Where(x => x.Status.Id == "2").ToList();
            CourseStatus upcomingStatus = _context.CourseStatuses.FirstOrDefault(x => x.Id == "1");
            CourseStatus activeStatus = _context.CourseStatuses.FirstOrDefault(x => x.Id == "2");
            CourseStatus completeStatus = _context.CourseStatuses.FirstOrDefault(x => x.Id == "3");
            CourseStatus erroredStatus = _context.CourseStatuses.FirstOrDefault(x => x.Id == "4");

            foreach (var item in upcomingCourses)
            {
                if (item.StartDate.Date <= now.Date)
                {
                    item.Status = activeStatus;
                    item.StatusId = activeStatus.Id;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            foreach (var item in activeCourses)
            {
                if (item.EndDate.Date < now.Date)
                {
                    item.Status = completeStatus;
                    item.StatusId = completeStatus.Id;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
            }

            var result = await _context.LearnCourses.Include(l => l.Status).Include(u => u.Users).ToListAsync();
            return result;
        }
    }
}
