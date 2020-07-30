using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tms.Infra.CrossCutting.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TmsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<TaskDto> Get(int id)
        {
            return new TaskDto() { Name = "Teste" };
        }

        // POST api/<TmsController>
        [HttpPost]
        public ActionResult Post(TaskDto taskDto)
        {
            return Ok();
        }

        //// GET: api/<TmsController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<TmsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<TmsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<TmsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TmsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
