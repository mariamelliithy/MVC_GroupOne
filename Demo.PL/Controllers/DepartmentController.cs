using Demo.BLL.Dtos.Departments;
using Demo.BLL.Services.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    //DepartmentController : Inhiertance [is a controller]
    //DepartmentController : Composition [has a department service]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment env) 
        {
            _departmentService = departmentService;
            _logger = logger;
            _env = env;
        }
        //Action ==> master action
        //Get : baseUrl/Department
        //Get : baseUrl/Department/Index
        [HttpGet]
        public IActionResult Index()
        {
            var department = _departmentService.GetAllDepartments();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentToCreateDto departmentDto) 
        {
            if(!ModelState.IsValid) 
                return View(departmentDto);
            var message = string.Empty;
            try
            {
                var result = _departmentService.CreateDepartment(departmentDto);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Department can not be created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentDto);
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentDto);
                }
                else
                {
                    message = "Department can not be created";
                    return View(departmentDto);
                }
            }
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
                return BadRequest(); //400
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound(); //404
            return View(department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
                return NotFound();
            return View(new DepartmentEditViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate,
            });
        }
        [HttpPost]
        public IActionResult Edit(int id ,DepartmentEditViewModel departmentVM)
        {
            if(!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var result = _departmentService.UpdateDepartment(new DepartmentToUpdateDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate,
                });
                if(result >  0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Department can not be updated";
                }
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message :  "Department can not be updated";
            }
            return View(departmentVM);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
                return NotFound();
            return View(department);

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _departmentService.DeleteDepartment(id);
            var message = string.Empty;
            try
            {
                if (result)
                    return RedirectToAction(nameof(Index));
                message = "An error happend when deleting the department";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        }
    }
}
