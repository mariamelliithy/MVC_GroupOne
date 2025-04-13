using Demo.BLL.Dtos.Employees;
using Demo.DAL.Entities.Common.Enums;
using Demo.DAL.Entities.Identity;
using Demo.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public UserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string SearchValue)
        {
            var userQuery = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(SearchValue))
            {
                userQuery = userQuery.Where(U => U.Email.ToLower().Contains(SearchValue.ToLower()));
            }
            var userList = await userQuery.Select(
                U => new UserViewModel
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email
                }
                ).ToListAsync();
            foreach ( var user in userList )
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }
            return View(userList);
        }
    
    #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id is null)
                return BadRequest(); //400
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(); //404
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(userViewModel);
        }
        #endregion

    #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            return View(new UserViewModel
            {
                Id = user.Id,
                FName= user.FName,
                LName= user.LName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Edit(string id, UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return View(userViewModel);
            var message = string.Empty;
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if(user is null)
                    return NotFound();
                user.FName = userViewModel.FName;
                user.LName = userViewModel.LName;
                user.Email = userViewModel.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    message = "User can't be updated";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Employee can not be updated";
            }
            return View(userViewModel);
        }
        #endregion

    #region Delete
        //[HttpGet]
        //public async Task<IActionResult> Delete(string? id)
        //{
        //    if (id is null)
        //        return BadRequest();
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user is null)
        //        return NotFound();
        //    return View(new UserViewModel{
        //        Email = user.Email,
        //        FName = user.FName,
        //        LName = user.LName,
        //        Id = id
        //    });

        //}
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var message = string.Empty;
            try
            {
                if (user is not null)
                {
                    await _userManager.DeleteAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                message = "An error happened while deleting the user";

            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the user";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
        #endregion
    }
    }
