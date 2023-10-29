using System;
using EmployeeApi.Repositories.Context;
using EmployeeApi.EFEmployee;
using EmployeeApi.Helpers;
using Microsoft.Extensions.Options;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeApi.Repositories
{
    public interface IEmployeeRepository
    {
        public Employee GetEmployeeByNikAndPass(string nik, string pass);
        public Employee GetById(Guid employeeId);
        public EmployeeDetail GetEmployeeDetail(Guid employeeId);
        public List<Employee> GetAll();
        public void DeleteEmployee(Guid employeeId);
        public void DeleteEmployeeDetail(Guid employeeId);
        public void Update(EFEmployee.Employee emp);
        public void InsertEmployee(EFEmployee.Employee emp, string createdBy);
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;
        private readonly IOptions<AppSettings> _config;

        public EmployeeRepository(DapperContext context, IOptions<AppSettings> config)
        {
            _context = context;
            _config = config;
        }
        public Employee GetEmployeeByNikAndPass(string nik, string pass)
        {
            string sql = "SELECT TOP 1 * FROM Employee with(nolock) WHERE Nik = @Nik and password = @Pass and IsDeleted = 0";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var employee = connection.Query<Employee>(sql, new { Nik = nik, Pass = pass });
                    return employee.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public Employee GetById(Guid employeeId)
        {
            string sql = "SELECT TOP 1 * FROM Employee with(nolock) WHERE Id = @empID and IsDeleted = 0";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var employee = connection.Query<Employee>(sql, new { empID = employeeId });
                    return employee.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public EmployeeDetail GetEmployeeDetail(Guid employeeId)
        {
            string sql = "SELECT TOP 1 * FROM EmployeeDetail with(nolock) WHERE EmployeeId = @EmpId and IsDeleted = 0";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var employeeDetail = connection.Query<EmployeeDetail>(sql, new { EmpId = employeeId });
                    return employeeDetail.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<Employee> GetAll()
        {
            string sql = "SELECT * FROM Employee with(nolock)";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var employee = connection.Query<Employee>(sql);
                    return employee.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void DeleteEmployee(Guid employeeId)
        {
            string sql = "update Employee set IsDeleted = 1 WHERE Id = @EmpId";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var isSuccess = connection.Execute(sql, new { EmpId = employeeId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void DeleteEmployeeDetail(Guid employeeId)
        {
            string sql = "update EmployeeDetail set IsDeleted = 1 WHERE EmployeeId = @EmpId";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var isSuccess = connection.Execute(sql, new { EmpId = employeeId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void Update(EFEmployee.Employee emp)
        {
            string sql = "update Employee set Nik = @Nik, Role = @Role WHERE Id = @EmpId update EmployeeDetail set Name = @Name, MobilePhone = @MobilePhone, JobClass = @JobClass, Department = @Department WHERE EmployeeId = @EmpId";

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var isSuccess = connection.Execute(sql, new { EmpId = emp.ID, Nik = emp.Nik, Role = emp.Role, Name = emp.EmployeeDetail.Name, MobilePhone = emp.EmployeeDetail.MobilePhone, JobClass = emp.EmployeeDetail.JobClass, Department = emp.EmployeeDetail.Department });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void InsertEmployee(EFEmployee.Employee emp, string createdBy)
        {
            Guid id = Guid.NewGuid();
            string sqlEmp = "insert into Employee (Id, Nik, Password, Role, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES (@Id, @Nik, @Password, @Role, 0, GETDATE(), @CreatedBy, GETDATE(), @CreatedBy )";
            string sqlEmpDet = "insert into EmployeeDetail (EmployeeId, Name, MobilePhone, JobClass, Department, JoinDate, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) VALUES (@Id, @Name, @MobilePhone, @Jobclass,  @Department, @JoinDate, 0, GETDATE(), @CreatedBy, GETDATE(), @CreatedBy )";
            string sql = sqlEmp + " " + sqlEmpDet;

            using (var connection = _context.CreateMasterConnection())
            {
                try
                {
                    var isSuccess = connection.Execute(sql, new { Id = id, CreatedBy = createdBy, Nik = emp.Nik, Password = emp.Password, Role = emp.Role, Name = emp.EmployeeDetail.Name, MobilePhone = emp.EmployeeDetail.MobilePhone, JobClass = emp.EmployeeDetail.JobClass, Department = emp.EmployeeDetail.Department, JoinDate = emp.EmployeeDetail.JoinDate });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

    }
}

