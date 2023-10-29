using EmployesCRUD.Database.DomainModels;

namespace EmployesCRUD.ViewModels
{
    public class EmployeeUpdateResponseViewModel : BaseEmployeeViewModel
    {
        public int Id { get; set; }

        public List<Department> Departments { get; set; }
    }
}
