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
    private bool check;

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
        return View("Views/Admin/Employes/Employes.cshtml", _employeeRepository.GetAllWithDepartaments());
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

        //while (check)
        //{
        //    Random rand = new Random();
        //    int randNum = rand.Next(10000,99999);
        //    string employeecode = "E" + randNum.ToString();

        //    if (!employee.Any(e => e.Employeecode == employeecode))
        //    {
        //        employee.Employeecode = employeecode;
        //        check = false;
        //    };

        //    employee.Add(employee);
        //}

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

    [HttpGet("edit")]
    public IActionResult Edit(int id)
    {
        Employes employes = _employeeRepository.GetById(id);
        if (employes == null)
            return NotFound();


        var model = new EmployeeUpdateResponseViewModel
        {
            Id = employes.id,
            Name = employes.Name,
            Surname = employes.Surname,
            FatherName = employes.FatherName,
            FINcode = employes.FINcode,
            Email = employes.EmailAdress,
            Departments = _departmentRepository.GetAll(),
            DepartmentId = employes.DepartmentId
        };

        return View("Views/Admin/Employes/EmployesEdit.cshtml", model);
    }

    [HttpPost("edit")]
    public IActionResult Edit(EmployeeUpdateRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return PrepareValidationView("Views/Admin/Employes/EmployesEdit.cshtml");

        if (model.DepartmentId != null)
        {
            var department = _departmentRepository.GetById(model.DepartmentId.Value);
            if (department == null)
            {
                ModelState.AddModelError("DepartmentId", "Department doesn't exist");

                return PrepareValidationView("Views/Admin/Employes/EmployesAdd.cshtml");
            }
        }

        Employes employes = _employeeRepository.GetById(model.Id);
        if (employes == null)
            return NotFound();


        employes.Name = model.Name;
        employes.Surname = model.Surname;
        employes.FatherName = model.FatherName;
        employes.FINcode = model.FINcode;
        employes.EmailAdress = model.Email;
        employes.DepartmentId = model.DepartmentId;


        try
        {
            _employeeRepository.Update(employes);
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Postgresql Exception");

            throw e;
        }


        return RedirectToAction("Employes");
    }

    [HttpGet("delete")]
    public IActionResult Delete(int id)
    {
        Employes employes = _employeeRepository.GetById(id);
        if (employes == null)
        {
            return NotFound();
        }

        _employeeRepository.RemoveById(id);

        return RedirectToAction("Employes");
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
