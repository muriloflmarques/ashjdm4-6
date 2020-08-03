using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.Data.Interface;
using Tms.Service;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error while deleting Task, informed Id didn't return any result")]
        public void test_ThrowException_DeleteTask_Service_Task_Is_Null()
        {
            //Returns a Task with Id = 1
            var task = this.GetFunctionalTask();

            var taskRepository = new Mock<ITaskRepository>();

            //For any given Id it returns Null
            taskRepository.Setup(rep => rep.SelectById(It.IsAny<int>()))
                .Returns(null as Domain.Task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.DeleteTask(0);
        }

        [TestMethod]
        public void test_ThrowException_DeleteTask_Service_Task_Without_SubTask()
        {
            //Returns a Task with Id = 1
            var task = this.GetFunctionalTask();

            var taskRepository = new Mock<ITaskRepository>();

            //For any given Id it returns Null
            taskRepository.Setup(rep => rep.SelectById(task.Id))
                .Returns(task);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.DeleteTask(task.Id);
        }

        [TestMethod]
        public void test_ThrowException_DeleteTask_Service_Task_With_SubTask()
        {
            //Returns a Parent Task with Id = 1 and a Child (inside the Parent) with Id = 2
            var subTask = this.GetFunctionalSubTask();

            var taskRepository = new Mock<ITaskRepository>();

            //For any given Id it returns Null
            taskRepository.Setup(rep => rep.SelectById(subTask.ParentTask.Id))
                .Returns(subTask.ParentTask);

            var taskService = new TaskService(new Mock<IUnitOfWork>().Object, taskRepository.Object);

            taskService.DeleteTask(subTask.ParentTask.Id);
        }
    }
}