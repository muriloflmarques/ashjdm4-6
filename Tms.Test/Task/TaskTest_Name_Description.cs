using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using Tms.Domain;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;

namespace Tms.Test.Task
{
    [TestClass]
    public partial class TaskTest
    {
        #region Private Fields

        private readonly int maxLengthTaskName = 40;
        private readonly int minLengthTaskName = 3;

        private readonly int maxLengthTaskDescription = 200;

        #endregion

        #region Private Methods

        private void SetId(object obj, int id) =>
            new PrivateObject(obj).SetPrivateProperty("Id", id);

        private Domain.Task GetFunctionalTask()
        {
            const string taskName = "A Perfect Name";
            const string taskDescription = "A Perfect Description";

            return new Domain.Task(taskName, taskDescription);
        }

        private SubTask GetFunctionalSubTask()
        {
            var parentTask = this.GetFunctionalTask();
            var childTask = this.GetFunctionalTask();

            return new SubTask(
                    parentTask: parentTask,
                    childTask: childTask);
        }

        private void AddFunctionalSubTask_IntoParentTask(Domain.Task parentTask)
        {
            if (parentTask != null)
            {
                int childTasknumber = (parentTask.SubTasks?.Count ?? 0) + 1;

                string childTaskName = $"A Perfect Name - Child nº {childTasknumber}";
                string childTaskDescription = $"A Perfect Description - Child nº {childTasknumber}";

                var childTask = new Domain.Task(childTaskName, childTaskDescription);

                var subTask = new SubTask(
                    parentTask: parentTask,
                    childTask: childTask);

                parentTask.AddSubTask(subTask);
            }
        }

        private Domain.Task GetFunctionalTask_WithSubTasks(int subTaskAmount)
        {
            var task = this.GetFunctionalTask();

            if (subTaskAmount > 0)
            {
                for (int i = 0; i < subTaskAmount; i++)
                {
                    this.AddFunctionalSubTask_IntoParentTask(task);
                }
            }

            return task;
        }

        #endregion

        #region Using Constructor

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "A name is needed when creating a new Task")]
        public void test_ThrowException_TaskName_Null()
        {
            const string taskName = null;
            const string taskDescription = "A Perfect Description";

            var task = new Domain.Task(taskName, taskDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not be longer than 40 characteres")]
        public void test_ThrowException_TaskName_TooLong()
        {
            string taskName = new string('A', this.maxLengthTaskName + 1); ;
            const string taskDescription = "A Perfect Description";

            var task = new Domain.Task(taskName, taskDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not have less than 3 characteres")]
        public void test_ThrowException_TaskName_TooShort()
        {
            string taskName = new string('A', this.minLengthTaskName - 1); ;
            const string taskDescription = "A Perfect Description";

            var task = new Domain.Task(taskName, taskDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "A description is needed when creating a new Task")]
        public void test_ThrowException_TaskDescription_Null()
        {
            const string taskName = "A Perfect Name";
            const string taskDescription = null;

            var task = new Domain.Task(taskName, taskDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not be longer than 200 characteres")]
        public void test_ThrowException_TaskDescription_TooLong()
        {
            const string taskName = "A Perfect Name";
            string taskDescription = new string('A', this.maxLengthTaskDescription + 1); ;

            var task = new Domain.Task(taskName, taskDescription);
        }

        #endregion

        #region Using Update Method

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "A name is needed when creating a new Task")]
        public void test_ThrowException_UpdatingTask_TaskName_Null()
        {
            var task = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = null,
                Description = "A Perfect Description"
            };

            task.UpdateNameAndDescription(creatingTaskDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not be longer than 40 characteres")]
        public void test_ThrowException_UpdatingTask_TaskName_TooLong()
        {
            var task = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = new string('A', this.maxLengthTaskName + 1),
                Description = "A Perfect Description"
            };

            task.UpdateNameAndDescription(creatingTaskDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not have less than 3 characteres")]
        public void test_ThrowException_UpdatingTask_TaskName_TooShort()
        {
            var task = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = new string('A', this.minLengthTaskName - 1),
                Description = "A Perfect Description"
            };

            task.UpdateNameAndDescription(creatingTaskDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "A description is needed when creating a new Task")]
        public void test_ThrowException_UpdatingTask_TaskDescription_Null()
        {
            var task = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = "A Perfect Name",
                Description = null
            };

            task.UpdateNameAndDescription(creatingTaskDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "The Task's name can not be longer than 200 characteres")]
        public void test_ThrowException_UpdatingTask_TaskDescription_TooLong()
        {
            var task = this.GetFunctionalTask();

            var creatingTaskDto = new CreatingTaskDto()
            {
                Name = "A Perfect Name",
                Description = new string('A', this.maxLengthTaskDescription + 1)
            };

            task.UpdateNameAndDescription(creatingTaskDto);
        }

        #endregion
    }
}