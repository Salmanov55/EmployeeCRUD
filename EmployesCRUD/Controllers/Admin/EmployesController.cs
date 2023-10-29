using EmployesCRUD.Database.DomainModels;
using EmployesCRUD.Database.Repositories;
using EmployesCRUD.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace EmployesCRUD.Controllers.Admin;

[Route("admin/employee")]
public class EmployesController : Controller
{
    private readonly EmployesRepository _employeeRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly ILogger<EmployesController> _logger;

    public EmployesController()
    {
        _employeeRepository = new EmployesRepository();
        _departmentRepository = new DepartmentRepository();

        var factory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        _logger = factory.CreateLogger<EmployesController>();
    }

    [HttpGet] 
    public IActionResult Products()
    {
        return View("Views/Admin/Employes/Employes.cshtml", _employeeRepository.GetAllWithCategories());
    }

    [HttpGet("add")]
    public IActionResult Add()
    {
        var departments = _departmentRepository.GetAll();
        var model = new EmployeeAddResponseViewModel
        {
            DepartmentList = departments
        };

        return View("Views/Admin/Employes/EmployesAdd.cshtml", model);
    }

    [HttpPost("add")]
    public IActionResult Add(EmployeeAddRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return PrepareValidationView("Views/Admin/Employes/EmployesAdd.cshtml");

        if (model.DepartmentId != null)
        {
            var department = _departmentRepository.GetById(model.DepartmentId.Value);
            if (department == null)
            {
                ModelState.AddModelError("DepartmentId", "Department doesn't exist");

                return PrepareValidationView("Views/Admin/Employes/EmployesAdd.cshtml");
            }
        }

        var employee = new Employes
        {
            Name = model.Name,
            Surname = model.Surname,
            FatherName = model.FatherName,
            FINcode = model.FINcode,
            EmailAdress =model.Email,
            DepartmentId = model.DepartmentId
        };

        try
        {
            _employeeRepository.Insert(employee);
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Postgresql Exception");

            throw e;
        }

        return RedirectToAction("Products");
    }
    private IActionResult PrepareValidationView(string viewName)
    {
        var departments = _departmentRepository.GetAll();

        var responseViewModel = new EmployeeAddResponseViewModel
        {
            DepartmentList = departments
        };

        return View(viewName, responseViewModel);
    }

    protected override void Dispose(bool disposing)
    {
        _employeeRepository.Dispose();
        _departmentRepository.Dispose();

        base.Dispose(disposing);
    }
}
