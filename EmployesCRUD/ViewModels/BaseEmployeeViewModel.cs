using System.ComponentModel.DataAnnotations;

namespace EmployesCRUD.ViewModels;

public class BaseEmployeeViewModel
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "The name must be at least 3 and at most 20 characters.")]
    public string Name { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "The surname must be at least 3 and at most 20 characters.")]
    public string Surname { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "The fathername must be at least 3 and at most 20 characters.")]
    public string FatherName { get; set; }

    [RegularExpression(@"^[A-Za-z0-9]{7}$", ErrorMessage = "FIN code must consist of 7 letters and numbers.")]
    public string FINcode { get; set; }

    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }
    public int DepartmentId { get; set; }
}
