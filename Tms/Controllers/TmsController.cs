using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tms.Domain.PrivateMapper;
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

        /// <summary>
        /// Shows Tasks without Join with SubTasks
        /// </summary>
        // GET api/<tmscontroller>/GetTasksWithoutSubtasks
        [HttpGet("GetTasksWithoutSubtasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasksWithoutSubtasks()
        {
            var taskDtoList = _taskService.SelectTasksWithoutSubtasks();

            return Ok(taskDtoList);
        }

        /// <summary>
        /// Shows Tasks with Join with SubTasks excluding from the root the SubTasks
        /// </summary>
        // GET api/<tmscontroller>/TasksWithSubtasks
        [HttpGet("TasksWithSubtasks")]
        public ActionResult<IEnumerable<TaskDto>> GetTasksWithSubtasks()
        {
            var taskDtoList = _taskService.SelectTasksWithSubtasks();

            return Ok(taskDtoList);
        }

        /// <summary>
        /// Get especific Task
        /// </summary>
        // GET api/<tmscontroller>/1
        [HttpGet("{id}")]
        public ActionResult<TaskDto> Get(int id)
        {
            var taskDto = _taskRepository.SelectById(id)?.MapToDto();

            return Ok(taskDto);
        }

        /// <summary>
        /// Creates new Task
        /// </summary>
        // POST api/<TmsController>/NewTask
        [HttpPost("NewTask")]
        public IActionResult Post(CreatingTaskDto creatingTaskDto)
        {
            TaskDto taskDto;

            if (creatingTaskDto.ParentTaskId > 0)
                taskDto = _taskService.CreateNewSubTask(creatingTaskDto, commit: true);
            else
                taskDto = _taskService.CreateNewTask(creatingTaskDto, commit: true);

            return Ok(taskDto);
        }

        /// <summary>
        /// Update a Task
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CreatingTaskDto creatingTaskDto)
        {
            var taskDto = _taskService.UpdateTask(id, creatingTaskDto, commit: true);

            return Ok(taskDto);
        }

        /// <summary>
        /// Update a Task's State
        /// </summary>
        // PUT api/<TmsController>/1
        [HttpPut("{id}/{taskState}")]
        public ActionResult Put(int id, int taskState)
        {
            if (!Enum.IsDefined(typeof(TaskStateEnum), taskState))
                throw new DomainRulesException($"The informed Task State ({taskState}) is not valid");

            var taskDto = _taskService.ChangeTaskState(id, (TaskStateEnum)taskState, commit: true);

            return Ok(taskDto);
        }

        /// <summary>
        /// Remove a Task
        /// </summary>
        // DELETE api/<tmscontroller>/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _taskService.DeleteTask(id);

            //Commit the changes done by the Service
            _uow.Commit();

            return Ok();
        }
    }
}