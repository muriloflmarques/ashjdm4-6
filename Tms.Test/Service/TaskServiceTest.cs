using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Tms.Service.Interfaces;
using Tms.Service;
using Tms.Infra.Data.Interface;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {

        [TestMethod]
        //[ExpectedException(typeof(DomainRulesException), "You must informe a start date before setting a finish date")]
        public void test_ChangeSubTaskStatse_From_Planned_To_Completed()
        {
            var uow = new Mock<IUnitOfWork>();
            var taskRepository = new Mock<ITaskRepository>();

            //taskRepository.Setup(rep => rep.GetDbSetAsNoTracking().Provider);

            //taskRepository.Setup(rep =>
            //    rep.AddDefaultIncludeIntoDbSet(
            //        taskRepository.Object.GetDbSetAsNoTracking()).Provider);

            var task = this.GetFunctionalTask();
            new PrivateObject(task).SetPrivateProperty("Id", 1);

            taskRepository.Setup(rep => rep.SelectById(1)).Returns(task);

            var taskService = new TaskService(uow.Object, taskRepository.Object);

            taskService.ChangeTaskState(1, TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);

            //var subTask = this.GetFunctionalSubTask_As_Planned();

            //subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);
        }

    }
}
