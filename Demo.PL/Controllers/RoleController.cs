using Demo.DAL.Entities.Identity;
using Demo.PL.ViewModels.Roles;
using Demo.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string SearchValue)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(SearchValue))
            {
                rolesQuery = rolesQuery.Where(U => U.Name.ToLower().Contains(SearchValue.ToLower()));
            }
            var rolesList = await rolesQuery.Select(
                U => new RoleViewModel
                {
                    Id = U.Id,
                    Name = U.Name
                }
                ).ToListAsync();
            return View(rolesList);
        }

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id is null)
                return BadRequest(); //400
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(); //404
            var roleViewModel = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleViewModel);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            var users = await _userManager.Users.ToListAsync();
            return View(new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = users.Select(user => new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Edit(string id, RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
                return View(roleViewModel);
            var message = string.Empty;
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound();
                role.Name = roleViewModel.Name;
                var result = await _roleManager.UpdateAsync(role);
                foreach(var userRole in roleViewModel.Users)
                {
                    var user = await _userManager.FindByIdAsync(userRole.UserId);
                    if(user is not null)
                    {
                        if(userRole.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                        else if(!userRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    message = "Role can't be updated";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Role can not be updated";
            }
            return View(roleViewModel);
        }
        #endregion

        #region Delete
        
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var message = string.Empty;
            try
            {
                if (role is not null)
                {
                    await _roleManager.DeleteAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                message = "An error happened while deleting the Role";

            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the role";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = roleViewModel.Name,
                });
                return RedirectToAction(nameof(Index));
            }
            return View(roleViewModel);
        } 
        #endregion
    }
}
