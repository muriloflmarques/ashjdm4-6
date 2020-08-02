using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Private Methods

        private Domain.Task GetFunctionalTask_With_PlannedStatus() =>
            this.GetFunctionalTask();

        private Domain.Task GetFunctionalTask_With_InProgressStatus()
        {
            var task = this.GetFunctionalTask_With_PlannedStatus();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            return task;
        }

        private Domain.Task GetFunctionalTask_With_CompletedStatus()
        {
            var task = this.GetFunctionalTask_With_InProgressStatus();

            task.ChangeTaskState(TaskStateEnum.Completed);

            return task;
        }

        #endregion

        #region From Planned

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_Planned_To_Planned()
        {
            var task = this.GetFunctionalTask_With_PlannedStatus();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_Planned_To_InProgress()
        {
            var task = this.GetFunctionalTask_With_PlannedStatus();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "You must informe a start date before setting a finish date")]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_Planned_To_Completed()
        {
            var task = this.GetFunctionalTask_With_PlannedStatus();

            task.ChangeTaskState(TaskStateEnum.Completed);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_Planned_To_NonexistentStatus()
        {
            var task = this.GetFunctionalTask_With_PlannedStatus();

            task.ChangeTaskState((TaskStateEnum)(-1));

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        #endregion

        #region From In Progress

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_InProgress_To_Planned()
        {
            var task = this.GetFunctionalTask_With_InProgressStatus();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_InProgress_To_InProgress()
        {
            var task = this.GetFunctionalTask_With_InProgressStatus();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_InProgress_To_Completed()
        {
            var task = this.GetFunctionalTask_With_InProgressStatus();

            task.ChangeTaskState(TaskStateEnum.Completed);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Completed);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_InProgress_To_NonexistentStatus()
        {
            var task = this.GetFunctionalTask_With_InProgressStatus();

            task.ChangeTaskState((TaskStateEnum)(-1));

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        #endregion

        #region From Completed

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_Completed_To_Planned()
        {
            var task = this.GetFunctionalTask_With_CompletedStatus();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Task_Without_SubTasks_ChangeStatus_From_Completed_To_InProgress()
        {
            var task = this.GetFunctionalTask_With_CompletedStatus();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_Completed_To_Completed()
        {
            var task = this.GetFunctionalTask_With_CompletedStatus();

            task.ChangeTaskState(TaskStateEnum.Completed);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Completed);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void test_ThrowException_Task_Without_SubTasks_ChangeStatus_From_Completed_To_NonexistentStatus()
        {
            var task = this.GetFunctionalTask_With_CompletedStatus();

            task.ChangeTaskState((TaskStateEnum)(-1));

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress);
        }

        #endregion
    }
}