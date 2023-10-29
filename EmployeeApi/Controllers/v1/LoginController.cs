using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EmployeeApi.Services;

namespace EmployeeApi.Controllers
{
    [ApiVersion("1")]
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [Route("SimpleLogin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult SimpleLogin(string nik, string password="")
        {
            var response = _loginService.Get(nik, password);
            return Ok(response);
        }

        [Route("GetAllUser")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetAllUser()
        {
            var response = _loginService.GetAllUser();
            return Ok(response);
        }

        [Route("GetDetail")]
        [HttpGet]
        [MapToApiVersion("1")]
        public IActionResult GetDetail(System.Guid id)
        {
            var response = _loginService.GetDetailById(id);
            return Ok(response);
        }

        [Route("DeleteUser")]
        [HttpPut]
        [MapToApiVersion("1")]
        public IActionResult DeleteUser(System.Guid id)
        {
            var response = _loginService.DeleteUser(id);
            return Ok(response);
        }

        [Route("UpdateUser")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult UpdateUser(EFEmployee.Employee emp)
        {
            var response = _loginService.UpdateUser(emp);
            return Ok(response);
        }

        [Route("InsertUser")]
        [HttpPost]
        [MapToApiVersion("1")]
        public IActionResult InsertUser(EFEmployee.Employee emp, string createdBy)
        {
            var response = _loginService.InsertUser(emp, createdBy);
            return Ok(response);
        }

    }
}
