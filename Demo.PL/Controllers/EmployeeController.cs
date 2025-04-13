using Demo.BLL.Dtos.Employees;
using Demo.BLL.Services.Departments;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
        #region Services

        //EmployeeController : Inhiertance [is a controller]
        //EmployeeController : Composition [has a Employee service]
        public class EmployeeController : Controller
        {
            private readonly IEmployeeService _employeeService;
            private readonly ILogger<EmployeeController> _logger;
            private readonly IWebHostEnvironment _env;

            public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IWebHostEnvironment env)
            {
                _employeeService = employeeService;
                _logger = logger;
                _env = env;
            }
        #endregion

        #region Index

        //Action ==> master action
        //Get : baseUrl/Employee
        //Get : baseUrl/Employee/Index
        [HttpGet]
        public async Task<IActionResult> Index(string SearchValue)
        {
            var employee =await _employeeService.GetAllEmployees(SearchValue);
            return View(employee);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create(/*[FromServices] IDepartmentService departmentService*/)
        {
            //ViewData["Departments"] = departmentService.GetAllDepartments();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Create(EmployeeToCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);
            var message = string.Empty;
            try
            {
                var result =await _employeeService.CreateEmployee(employeeDto);
                if (result > 0)
                    TempData["Message"] = "Employee created successfully";
                else
                {
                    message = "Employee can not be created";
                    ModelState.AddModelError(string.Empty, message);

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employeeDto);
                }
                else
                {
                    message = "Employee can not be created";
                    return View(employeeDto);
                }
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest(); //400
            var employee =await _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
                return NotFound(); //404
            return View(employee);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var employee =await _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
                return NotFound();
            return View(new EmployeeToUpdateDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                IsActive = employee.IsActive,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Id = id.Value,
                Salary = employee.Salary,
                EmpolyeeType = Enum.TryParse<EmployeeType>(employee.EmpolyeeType, out var empType) ? empType : default,
                Gender = Enum.TryParse<Gender>(employee.Gender, out var gender) ? gender : default,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Edit(int id, EmployeeToUpdateDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);
            var message = string.Empty;
            try
            {
                var result =await _employeeService.UpdateEmployee(employeeDto);
                if (result > 0)
                    TempData["Message"] = "Employee Updated successfully";
                else
                {
                    message = "Employee can not be updated";
                    TempData["Message"] = message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Employee can not be updated";
            }
            return View(employeeDto);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var employee =await _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
                return NotFound();
            return View(employee);

        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Delete(int id)
        {
            var result =await _employeeService.DeleteEmployee(id);
            var message = string.Empty;
            try
            {
                if (result)
                    TempData["Message"] = "Employee Deleted successfully";
                message = "An error happend when deleting the employee";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the employee";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        } 
        #endregion

    }
}
