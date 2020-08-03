using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.Data.Interface;
using Tms.Service;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "To create a new SubTask a existing Task is needed")]
        public void test_ThrowException_CreateNewSubTask_Service_ParentTask_Is_Null()
        {
            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = validTaskName,
                Description = validTaskDescription,

                ParentTaskId = 1
            };

            var taskRepository = new Mock<ITaskRepository>();

            //For any given Id it returns Null
            taskRepository.Setup(rep => rep.SelectById(It.IsAny<int>()))
                .Returns(null as Domain.Task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.CreateNewSubTask(creatingTaskDto);
        }

        [TestMethod]
        public void test_CreateNewSubTask_Service()
        {
            //Returns a Task with Id = 1
            var parentTask = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = validTaskName,
                Description = validTaskDescription,

                ParentTaskId = parentTask.Id
            };

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Parent's Id then return Parent
            taskRepository.Setup(rep => rep.SelectById(parentTask.Id))
                .Returns(parentTask);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            //Check if Parent has NOT a SubTask
            Assert.IsTrue((parentTask.SubTasks?.Count ?? 0) == 0);

            taskService.CreateNewSubTask(creatingTaskDto);

            //Parent Task is still in memory because taskRepository use it's pointer
            //Check if Parent has a SubTask
            Assert.IsTrue((parentTask.SubTasks?.Count ?? 0) == 1);
        }
    }
}