namespace EmployesCRUD.Database.DomainModels
{
    public class Employes
    {
        public Employes() 
        {

        }

        public Employes(int id, string employeecode, string name, string surname, string fatherName, string fINcode, string emailAdress, int departmentId, Department department)
        {
            this.id = id;
            Employeecode = employeecode;
            Name = name;
            Surname = surname;
            FatherName = fatherName;
            FINcode = fINcode;
            EmailAdress = emailAdress;
            DepartmentId = departmentId;
            Department = department;
        }

        public int id { get; set; }
        public string Employeecode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string FINcode { get; set; }
        public string EmailAdress { get; set; }
        public int? DepartmentId { get; set; }
        public bool IsDeleted { get; set; }
        public Department Department { get; set; }

        internal bool Any(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
