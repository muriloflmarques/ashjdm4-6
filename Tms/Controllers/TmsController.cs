using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.CrossCutting.Enums;
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

        [HttpGet("GetTasksWithoutSubtasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasksWithoutSubtasks()
        {
            var tasks = _taskService.SelectTasksWithoutSubtasks();

            if (!tasks.Any())
                return Ok(null);

            var dtos = tasks
                ?.Select(task => { return _taskService.ConvertDomainToDto(task); })
                .ToList();
            
            return Ok(dtos);
        }

        [HttpGet("TasksWithSubtasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasksWithSubtasks()
        {
            var tasks = _taskService.SelectTasksWithSubtasks();

            if (!tasks.Any())
                return Ok(null);

            var dtos = tasks
                ?.Select(task => { return _taskService.ConvertDomainToDto(task); })
                .ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskDto> Get(int id)
        {
            var task = _taskRepository.SelectById(id);

            if (task == null)
                return Ok(null);

            var dto = _taskService.ConvertDomainToDto(task);

            return Ok(dto);
        }

        // POST api/<TmsController>/NewTask
        [HttpPost("NewTask")]
        public IActionResult Post(CreatingTaskDto creatingTaskDto)
        {
            if (creatingTaskDto.ParentTaskId > 0)
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

        // PUT api/<TmsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CreatingTaskDto creatingTaskDto)
        {
            _taskService.UpdateNewTask(id, creatingTaskDto);

            _uow.Commit();

            return Ok();
        }

        // PUT api/<TmsController>/5
        [HttpPut("{id}/{taskState}")]
        public ActionResult Put(int id, int taskState)
        {
            if (!Enum.IsDefined(typeof(TaskStateEnum), taskState))
                throw new DomainRulesException($"The informed Task State ({taskState}) is not valid");

            _taskService.ChangeTaskState(id, (TaskStateEnum)taskState);

            _uow.Commit();

            return Ok();
        }

        // delete api/<tmscontroller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _taskService.DeleteTask(id);

            _uow.Commit();

            return Ok();
        }
    }
}
