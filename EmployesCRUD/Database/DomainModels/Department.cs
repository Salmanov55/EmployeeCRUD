namespace EmployesCRUD.Database.DomainModels
{
    public class Department
    {
        public Department()
        {
        }

        public Department(int ıd, string name)
        {
            Id = ıd;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
