using EmployesCRUD.Database.DomainModels;
using Npgsql;

namespace EmployesCRUD.Database.Repositories
{
    public class DepartmentRepository : IDisposable
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public DepartmentRepository()
        {
            _npgsqlConnection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
            _npgsqlConnection.Open();
        }

        public List<Department> GetAll()
        {
            var selectQuery = "SELECT * FROM department ORDER BY department_name";

            using NpgsqlCommand command = new NpgsqlCommand(selectQuery, _npgsqlConnection);
            using NpgsqlDataReader dataReader = command.ExecuteReader();

            List<Department> department = new List<Department>();

            while (dataReader.Read())
            {
                Department employee = new Department
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Name = Convert.ToString(dataReader["name"]),
                };

                department.Add(employee);
            }

            return department;
        }

        public Department GetById(int id)
        {
            using NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM department WHERE id={id}", _npgsqlConnection);
            using NpgsqlDataReader dataReader = command.ExecuteReader();

            Department department = null;

            while (dataReader.Read())
            {
                department = new Department
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Name = Convert.ToString(dataReader["name"]),
                };
            }

            return department;
        }

        public void Dispose()
        {
            _npgsqlConnection.Dispose();
        }
    }
}
