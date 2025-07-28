namespace bibliotheque_back_end.Models.Service.Interface;

public class EmployeService: IEmployeService
{
    public IEnumerable<Employe> GetAllEmployees()
    {
        throw new NotImplementedException();
    }

    public Employe GetEmployeeById(int id)
    {
        throw new NotImplementedException();
    }

    public Employe GetEmployeeByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Employe AddEmployee(Employe newEmployee, string dataPassword)
    {
        throw new NotImplementedException();
    }

    public Employe UpdateEmployee(int id, Employe updatedEmployee)
    {
        throw new NotImplementedException();
    }

    public Employe UpdateEmployeeRole(int id, Role updatedRole)
    {
        throw new NotImplementedException();
    }

    public Employe DeleteEmployee(int id)
    {
        throw new NotImplementedException();
    }

    public bool employeeExists(int id)
    {
        throw new NotImplementedException();
    }
}