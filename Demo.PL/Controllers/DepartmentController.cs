using AutoMapper;
using Demo.BLL.Dtos.Departments;
using Demo.BLL.Services.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    //DepartmentController : Inhiertance [is a controller]
    //DepartmentController : Composition [has a department service]
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper, ILogger<DepartmentController> logger, IWebHostEnvironment env)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            _logger = logger;
            _env = env;
        }
        #endregion

        #region Index
        //Action ==> master action
        //Get : baseUrl/Department
        //Get : baseUrl/Department/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var department = await _departmentService.GetAllDepartments();
            return View(department);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                //DepartmentViewModel ==> DepartmentToCreateDto [Config]
                var departementToCreated = _mapper.Map<DepartmentViewModel, DepartmentToCreateDto>(departmentVM);
                var result =await _departmentService.CreateDepartment(departementToCreated);
                if (result > 0)
                {
                    TempData["Message"] = "Department created successfully";
                }
                else
                {
                    message = "Department can not be created";
                    TempData["Message"] = message;
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
                    return View(departmentVM);
                }
                else
                {
                    message = "Department can not be created";
                    return View(departmentVM);
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
            var department =await _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound(); //404
            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var department =await _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound();
            var departmentVM = _mapper.Map<DepartmentDetailsToReturnDto, DepartmentViewModel>(department);
            return View(departmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Edit(int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var departmentToUpdate = _mapper.Map<DepartmentToUpdateDto>(departmentVM);
                departmentToUpdate.Id = id;
                var result =await _departmentService.UpdateDepartment(departmentToUpdate);
                if (result > 0)
                    TempData["Message"] = "Department Updated successfully";
                else
                {
                    message = "Department can not be updated";
                    TempData["Message"] = message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Department can not be updated";
            }
            return View(departmentVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var department =await _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);

        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Action filter
        public async Task<IActionResult> Delete(int id)
        {
            var result =await _departmentService.DeleteDepartment(id);
            var message = string.Empty;
            try
            {
                if (result)
                {
                    TempData["Message"] = "Department Deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
                message = "An error happend when deleting the department";
                TempData["Message"] = message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "An error happend when deleting the department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));
        } 
        #endregion
    }
}
