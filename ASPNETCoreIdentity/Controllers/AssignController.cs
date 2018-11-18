using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCoreIdentity.Data;
using ASPNETCoreIdentity.Models;
using ASPNETCoreIdentity.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreIdentity.Controllers
{
    public class AssignController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AssignController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        //Displaying All users with role
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
           
            var roles = await _context.UserRoles.ToListAsync();
            var AssignedRoles = _context.Roles.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Name.ToString()
            }).ToList();
            AssignViewModels models = new AssignViewModels
            {
                //Data = usersWithRoles,
                Roles = AssignedRoles
            };


            return View(models);

        }
        [HttpPost]
        public async Task<IActionResult> Index(string UserId)
        {           
            var roles = _context.UserRoles.Where(x=>x.UserId == UserId).ToList();
            
            var AssignedRoles = _context.Roles.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Name.ToString(),
                Selected = roles.Any(x => x.RoleId == c.Id)
        }).ToList();

            AssignViewModels models = new AssignViewModels
            {
                UserId = UserId,
                Roles = AssignedRoles
            };

            ViewBag.Users = new SelectList(_context.Users, "Id", "Email", UserId);
            return View(models);
        }        

        //View Create Form
        public IActionResult Create()
        {
            UserWithRoleViewModels model = new UserWithRoleViewModels();
            model.Roles = _context.Roles.Select(c => new SelectListItem
            {
                Text = c.Name.ToString(),
                Value = c.Id.ToString()
            }).ToList();
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            return View(model);
        }
        // Assigning Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserWithRoleViewModels model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);//Getting User Details from Database
            var ExistingUserRoles = _context.UserRoles.Where(x => x.UserId == model.UserId).ToList();
            if(ExistingUserRoles != null)
            {
                foreach (var item in ExistingUserRoles)
                {
                    _context.UserRoles.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            List<SelectListItem> userRoles = model.Roles.Where(c => c.Selected).ToList();
            foreach (var role in userRoles)
            {
               await _userManager.AddToRoleAsync(User, role.Text);//Adding Role                    
            }
           

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string UserId, string RoleId)
        {
            var UserAndRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == UserId && x.RoleId == RoleId);
            _context.UserRoles.Remove(UserAndRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Displaying ModelState Error
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}