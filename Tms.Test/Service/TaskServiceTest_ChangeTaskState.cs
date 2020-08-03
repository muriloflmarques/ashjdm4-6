using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tms.Service;
using Tms.Infra.Data.Interface;
using Tms.Infra.CrossCutting.Enums;
using Tms.Infra.CrossCutting.CustomException;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error while changing Task's state, informed Id didn't return any result")]
        public void test_ThrowException_ChangeTaskState_Service_Task_Is_Null()
        {
            var taskRepository = new Mock<ITaskRepository>();

            //For any given Id it returns Null
            taskRepository.Setup(rep => rep.SelectById(It.IsAny<int>()))
                .Returns(null as Domain.Task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(1, TaskStateEnum.InProgress);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error while changing Task's state, parent Task not found")]
        public void test_ThrowException_ChangeSubTaskState_Service_ParentTask_Is_Null()
        {
            //Returns a Parent Task with Id = 1 and a Child (inside the Parent) with Id = 2
            var subTask = this.GetFunctionalSubTask();

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Parent's Id then return Null
            taskRepository.Setup(rep => rep.SelectById(subTask.ParentTask.Id))
                .Returns(null as Domain.Task);

            //When SelectById with Child's Id then return Child
            taskRepository.Setup(rep => rep.SelectById(subTask.ChildTask.Id))
                .Returns(subTask.ChildTask);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(subTask.ChildTask.Id, TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_Service_SubTask_To_InProgress()
        {
            //Returns a Parent Task with Id = 1 and a Child (inside the Parent) with Id = 2
            var subTask = this.GetFunctionalSubTask();

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Parent's Id then return Parent
            taskRepository.Setup(rep => rep.SelectById(subTask.ParentTask.Id))
                .Returns(subTask.ParentTask);

            //When SelectById with Child's Id then return Child
            taskRepository.Setup(rep => rep.SelectById(subTask.ChildTask.Id))
                .Returns(subTask.ChildTask);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(subTask.ChildTask.Id, TaskStateEnum.InProgress);

            //When any Children is as InProgress then the Parent is also as InProgress
            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.InProgress &&
               subTask.ChildTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_Service_SubTask_To_Completed()
        {
            //Returns a Parent Task with Id = 1 and a Child (inside the Parent) with Id = 2
            var subTask = this.GetFunctionalSubTask();

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Parent's Id then return Parent
            taskRepository.Setup(rep => rep.SelectById(subTask.ParentTask.Id))
                .Returns(subTask.ParentTask);

            //When SelectById with Child's Id then return Child
            taskRepository.Setup(rep => rep.SelectById(subTask.ChildTask.Id))
                .Returns(subTask.ChildTask);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(subTask.ChildTask.Id, TaskStateEnum.InProgress);
            taskService.ChangeTaskState(subTask.ChildTask.Id, TaskStateEnum.Completed);

            //When any Children is as InProgress then the Parent is also as InProgress
            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Completed &&
               subTask.ChildTask.TaskState == TaskStateEnum.Completed);
        }

        [TestMethod]
        public void test_ChangeTaskState_Service_Task_To_InProgress()
        {
            //Returns a Task with Id = 1
            var task = this.GetFunctionalTask();

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Task's Id then return Task
            taskRepository.Setup(rep => rep.SelectById(task.Id))
                .Returns(task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(task.Id, TaskStateEnum.InProgress);

            //When any Children is as InProgress then the Parent is also as InProgress
            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_ChangeTaskState_Service_Task_To_Completed()
        {
            //Returns a Task with Id = 1
            var task = this.GetFunctionalTask();

            var taskRepository = new Mock<ITaskRepository>();

            //When SelectById with Task's Id then return Task
            taskRepository.Setup(rep => rep.SelectById(task.Id))
                .Returns(task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.ChangeTaskState(task.Id, TaskStateEnum.InProgress);
            taskService.ChangeTaskState(task.Id, TaskStateEnum.Completed);

            //When any Children is as InProgress then the Parent is also as InProgress
            Assert.IsTrue(task.TaskState == TaskStateEnum.Completed);
        }
    }
}