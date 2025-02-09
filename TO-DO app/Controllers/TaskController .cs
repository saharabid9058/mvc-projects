using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{   public class TaskController : Controller
    {
        private readonly ToDoDbContext _context;
        public TaskController(ToDoDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Index()
        {
            var tasks = _context.tasks.ToList(); 
            return View(tasks);
        }
        public IActionResult ToDoTasks()
        {
            var tasks = _context.tasks.Where(t => t.Status == "Pending" || t.Status == "Missing").ToList();
            return View(tasks);
        }
        public IActionResult CompletedTasks()
        {
            var completedTasks = _context.tasks.Where(t => t.Status == "Completed").ToList();
            var overdueTasks = _context.tasks
                .Where(t => t.Status == "Pending" && t.DueDate <= DateTime.Today)
                .ToList();
            foreach (var task in overdueTasks)
            {
                task.Status = "Completed";
            }
            _context.SaveChanges();
            completedTasks = _context.tasks.Where(t => t.Status == "Completed").ToList();
             return View(completedTasks);  
        }
        public IActionResult MissingTasks()
        {
            var today = DateTime.Today;
            var missingTasks = _context.tasks
                .Where(t => t.Status == "Pending" && t.DueDate < today)
                .ToList();
            foreach (var task in missingTasks)
            {
                task.Status = "Missing";
            }
            _context.SaveChanges(); 
            var updatedMissingTasks = _context.tasks.Where(t => t.Status == "Missing").ToList();
            return View(updatedMissingTasks); 
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                if (task.DueDate < DateTime.Today)
                {
                    task.Status = "Missing"; 
                }
                else
                {
                    task.Status = "Pending";  
                }
                _context.tasks.Add(task); 
                _context.SaveChanges();    
                return RedirectToAction("Dashboard");  
            }
            return View(task);  
        }
        public IActionResult Edit(int id)
        {
            var task = _context.tasks.Find(id);
            if (task == null)
            {
                return NotFound();  
            }
            return View(task);
        }
        [HttpPost]
        public IActionResult Edit(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                if (task.DueDate < DateTime.Today)
                {
                    task.Status = "Missing"; 
                }
                else if (task.Status == "Completed")
                {
                    task.Status = "Completed";  
                }
                else
                {
                    task.Status = "Pending";  
                }
                _context.tasks.Update(task);  
                _context.SaveChanges(); 
                return RedirectToAction("Dashboard"); 
            }
            return View(task);  
        }
        public IActionResult Delete(int id)
        {
            var task = _context.tasks.Find(id);
            if (task == null)
            {
                return NotFound();  
            }
            return View(task);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.tasks.Find(id);
            if (task != null)
            {
                _context.tasks.Remove(task);  
                _context.SaveChanges(); 
            }
            return RedirectToAction("Dashboard");  
        }
    }
}
