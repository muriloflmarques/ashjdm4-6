using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.Data.Interface;
using Tms.Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TmsController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _uow;

        public TmsController(ITaskService taskService, ITaskRepository taskRepository,
            IUnitOfWork uow)
        {
            this._taskService = taskService;
            this._taskRepository = taskRepository;
            this._uow = uow;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskDto>> Get()
        {
            var tasks = _taskRepository.SelectTop();

            var dtos = tasks
                ?.Select(task => { return _taskService.ConvertDomainToDto(task); })
                .ToList();

            return dtos;
        }

        [HttpGet("{id}")]
        public ActionResult<TaskDto> Get(int id)
        {
            var task = _taskRepository.SelectById(id);
            var dto = _taskService.ConvertDomainToDto(task);

            return dto;
        }

        // POST api/<TmsController>/NewTask
        [HttpPost("NewTask")]
        public ActionResult Post(CreatingTaskDto creatingTaskDto)
        {
            if (creatingTaskDto.ParentTaskId <= 0)
                _taskService.CreateNewSubTask(creatingTaskDto);
            else
                _taskService.CreateNewTask(creatingTaskDto);

            _uow.Commit();

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
