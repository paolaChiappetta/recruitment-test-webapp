using InterviewTest.Model;
using InterviewTest.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeStore store;

        public EmployeesController(IEmployeeStore store)
        {
            this.store = store;
        }

        [HttpGet]
        public List<Employee> Get()
        {
            return store.GetAllEmployees();
        }

        [HttpDelete]
        public void Delete([FromBody] int id)
        {
            store.DeleteEmployee(id);
        }

        // TODO: Update & Add
    }
}
