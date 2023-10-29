using EmployesCRUD.Database.DomainModels;
using Npgsql;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EmployesCRUD.Database.Repositories
{
    public class EmployesRepository : IDisposable
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public EmployesRepository()
        {
            _npgsqlConnection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
            _npgsqlConnection.Open();
        }


        public List<Employes> GetAll()
        {
            var selectQuery = "SELECT * FROM employees ORDER BY name";

            using NpgsqlCommand command = new NpgsqlCommand(selectQuery, _npgsqlConnection);
            using NpgsqlDataReader dataReader = command.ExecuteReader();

            List<Employes> employes = new List<Employes>();

            while (dataReader.Read())
            {
                Employes employee = new Employes
                {
                    id = Convert.ToInt32(dataReader["id"]),
                    Name = Convert.ToString(dataReader["name"]),
                    Surname = Convert.ToString(dataReader["surname"]),
                    FatherName = Convert.ToString(dataReader["father name"]),
                    FINcode = Convert.ToString(dataReader["FIN"]),
                    EmailAdress = Convert.ToString(dataReader["email"]),
                    DepartmentId = dataReader["departmentid"] as int?
                };

                employes.Add(employee);
            }

            return employes;
        }

        public List<Employes> GetAllWithCategories()
        {
            var selectQuery = "SELECT \r\n    e.\"id\" AS employeesId,\r\n    e.\"name\" AS employeesName,\r\n    e.\"surname\" AS employeesSurname,\r\n\te.\"father_name\" AS employesFatherName,\r\n    d.\"department_id\" AS departmentId,\r\n    d.\"department_name\" AS departmentName\r\n\tFROM employees e\r\n\tLEFT JOIN department d ON e.\"department_id\" = d.\"department_id\"\r\n\tORDER BY e.\"name\";";

            using NpgsqlCommand command = new NpgsqlCommand(selectQuery, _npgsqlConnection);
            using NpgsqlDataReader dataReader = command.ExecuteReader();

            List<Employes> products = new List<Employes>();

            while (dataReader.Read())
            {
                Employes employes = new Employes
                {
                    id = Convert.ToInt32(dataReader["id"]),
                    Name = Convert.ToString(dataReader["name"]),
                    Surname = Convert.ToString(dataReader["surname"]),
                    FatherName = Convert.ToString(dataReader["father name"]),
                    FINcode = Convert.ToString(dataReader["FIN"]),
                    EmailAdress = Convert.ToString(dataReader["email"]),
                    DepartmentId = dataReader["departmentid"] as int ?,
                    Department = dataReader["departmentid"] is int
                        ? new Department(Convert.ToInt32(dataReader["categoryId"]), Convert.ToString(dataReader["categoryName"]))
                        : null
                };

                products.Add(employes);
            }

            return products;
        }

        public void Insert(Employes employee)
        {
            string updateQuery = 
                "INSERT INTO employees(name, surname, father_name, fin_code, email_adress, department_id)" +
                $"VALUES('{employee.Name}', '{employee.Surname}', '{employee.FatherName}', '{employee.FINcode}', '{employee.EmailAdress}', {employee.DepartmentId})";

            using NpgsqlCommand command = new NpgsqlCommand(updateQuery, _npgsqlConnection);
            command.ExecuteNonQuery();
        }

        public void Update(Employes employee)
        {
            var query =
                    $"UPDATE employees " +
                    $"SET name='{employee.Name}', 'surname={employee.Surname}', 'fathername={employee.FatherName}', 'fincode={employee.FINcode}', 'email={employee.EmailAdress}', departmentid={employee.DepartmentId}" + 
                    $"WHERE id={employee.id}"; 

            using NpgsqlCommand updateCommand = new NpgsqlCommand(query, _npgsqlConnection);
            updateCommand.ExecuteNonQuery();
        }

        public void RemoveById(int id)
        {
            var query = $"DELETE FROM employees WHERE id={id}";

            using NpgsqlCommand updateCommand = new NpgsqlCommand(query, _npgsqlConnection);
            updateCommand.ExecuteNonQuery();
        }



        public void Dispose()
        {
            _npgsqlConnection.Dispose();
        }
    }
}
