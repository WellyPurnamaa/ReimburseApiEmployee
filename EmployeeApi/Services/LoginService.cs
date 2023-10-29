using Microsoft.Extensions.Options;
using System;
using System.Net;
using EmployeeApi.Helpers;
using EmployeeApi.Models;
using EmployeeApi.Repositories;

namespace EmployeeApi.Services
{
    public interface ILoginService
    {
        public WebApiResponseModel Get(string nik, string password);
        public WebApiResponseModel GetAllUser();
        public WebApiResponseModel DeleteUser(Guid id);
        public WebApiResponseModel GetDetailById(Guid id);
        public WebApiResponseModel UpdateUser(EFEmployee.Employee emp);
        public WebApiResponseModel InsertUser(EFEmployee.Employee emp, string createdBy);
    }

    public class LoginService : ILoginService
    {
        private readonly IOptions<AppSettings> _config;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly Logging _logging;
        public LoginService(IOptions<AppSettings> config, Logging logging, IEmployeeRepository employeeSQLRepository)
        {
            _config = config;
            _employeeRepository = employeeSQLRepository;
            _logging = logging;
        }

        private DateTime DATE_START;

        public WebApiResponseModel Get(string nik, string password)
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;

            var key = ("th1sish4rdc0de" + nik + password).ToUpper();
            var keyHashMD5 = DataEncription.ComputeHash(key, "MD5").ToUpper();
            var encryptedPass = DataEncription.ComputeHash(keyHashMD5, "SHA1").ToUpper();

            var tempEmployee = _employeeRepository.GetEmployeeByNikAndPass(nik, encryptedPass);
            if (tempEmployee != null)
            {
                var tempEmpDetail = _employeeRepository.GetEmployeeDetail(tempEmployee.ID);
                if (tempEmpDetail != null)
                {
                    tempEmployee.EmployeeDetail = tempEmpDetail;
                }
                response.data = tempEmployee;
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "Success";
            }
            else
            {
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "Nik or Password not exist";
            }

            return response;
        }

        public WebApiResponseModel GetAllUser()
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;


            var tempEmployee = _employeeRepository.GetAll();
            if (tempEmployee != null)
            {
                response.data = tempEmployee;
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "Success";
            }
            else
            {
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "Nik or Password not exist";
            }

            return response;
        }

        public WebApiResponseModel GetDetailById(Guid id)
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;


            var tempEmployee = _employeeRepository.GetById(id);
            if(tempEmployee != null)
            {
                var tempEmpDetail = _employeeRepository.GetEmployeeDetail(tempEmployee.ID);
                if (tempEmpDetail != null)
                {
                    tempEmployee.EmployeeDetail = tempEmpDetail;
                }
                response.data = tempEmployee;
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "Success";
            }
            else
            {
                response.statusCode = ((int)HttpStatusCode.OK).ToString();
                response.message = "data not exist";
            }

            return response;
        }

        public WebApiResponseModel DeleteUser(Guid id)
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;

            _employeeRepository.DeleteEmployee(id);
            _employeeRepository.DeleteEmployeeDetail(id);
            response.data = new EFEmployee.Employee();
            response.statusCode = ((int)HttpStatusCode.OK).ToString();
            response.message = "Success";

            return response;
        }

        public WebApiResponseModel UpdateUser(EFEmployee.Employee emp)
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;

            _employeeRepository.Update(emp);
            response.data = new EFEmployee.Employee();
            response.statusCode = ((int)HttpStatusCode.OK).ToString();
            response.message = "Success";

            return response;
        }

        public WebApiResponseModel InsertUser(EFEmployee.Employee emp, string createdBy)
        {
            var response = new WebApiResponseModel();
            DATE_START = DateTime.Now;

            if (!string.IsNullOrEmpty(emp.Password))
            {
                var key = ("th1sish4rdc0de" + emp.Nik + emp.Password).ToUpper();
                var keyHashMD5 = DataEncription.ComputeHash(key, "MD5").ToUpper();
                var encryptedPass = DataEncription.ComputeHash(keyHashMD5, "SHA1").ToUpper();
                emp.Password = encryptedPass;
            }

            _employeeRepository.InsertEmployee(emp, createdBy);
            response.data = new EFEmployee.Employee();
            response.statusCode = ((int)HttpStatusCode.OK).ToString();
            response.message = "Success";

            return response;
        }

    }
}
