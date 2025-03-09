using Demo.BLL.Dtos.Employees;
using Demo.BLL.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
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
        //Action ==> master action
        //Get : baseUrl/Employee
        //Get : baseUrl/Employee/Index
        [HttpGet]
        public IActionResult Index()
        {
            var employee = _employeeService.GetAllEmployees();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeToCreateDto employeeDto) 
        {
            if(!ModelState.IsValid) 
                return View(employeeDto);
            var message = string.Empty;
            try
            {
                var result = _employeeService.CreateEmployee(employeeDto);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Employee can not be created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employeeDto);
                }
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
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
                return BadRequest(); //400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
                return NotFound(); //404
            return View(employee);
        }
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id is null)
        //        return BadRequest();
        //    var employee = _employeeService.GetEmployeeById(id.Value);
        //    if(employee is null)
        //        return NotFound();
        //    return View(new DepartmentEditViewModel()
        //    {
        //        Code = department.Code,
        //        Name = department.Name,
        //        Description = department.Description,
        //        CreationDate = department.CreationDate,
        //    });
        //}
        //[HttpPost]
        //public IActionResult Edit(int id ,DepartmentEditViewModel departmentVM)
        //{
        //    if(!ModelState.IsValid)
        //        return View(departmentVM);
        //    var message = string.Empty;
        //    try
        //    {
        //        var result = _departmentService.UpdateDepartment(new DepartmentToUpdateDto()
        //        {
        //            Id = id,
        //            Code = departmentVM.Code,
        //            Name = departmentVM.Name,
        //            Description = departmentVM.Description,
        //            CreationDate = departmentVM.CreationDate,
        //        });
        //        if(result >  0)
        //            return RedirectToAction(nameof(Index));
        //        else
        //        {
        //            message = "Department can not be updated";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = _env.IsDevelopment() ? ex.Message :  "Department can not be updated";
        //    }
        //    return View(departmentVM);
        //}
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if(employee is null)
                return NotFound();
            return View(employee);

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _employeeService.DeleteEmployee(id);
            var message = string.Empty;
            try
            {
                if (result)
                    return RedirectToAction(nameof(Index));
                message = "An error happend when deleting the employee";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the employee";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
    }
}
