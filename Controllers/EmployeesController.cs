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
        [Route("get")]
        public List<Employee> Get()
        {
            return store.GetAllEmployees();
        }

        [HttpDelete]
        [Route("delete")]
        public void Delete([FromBody] int id)
        {
            store.DeleteEmployee(id);
        }

        [HttpPut]
        [Route("update")]
        public void Update([FromBody] Employee employee)
        {
            store.UpdateEmployee(employee);
        }

        [HttpPost]
        [Route("add")]
        public void Add([FromBody] Employee employee)
        {
            store.AddEmployee(employee);
        }
    }
}
