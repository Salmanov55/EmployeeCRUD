using EmployesCRUD.Database.DomainModels;

namespace EmployesCRUD.ViewModels
{
    public class EmployeeAddResponseViewModel : BaseEmployeeViewModel
    {
        public List<Department> DepartmentList { get; set; }
    }
}
