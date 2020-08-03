using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Private Methods

        private Domain.Task GetFunctionalTask_As_Planned() =>
            this.GetFunctionalTask();

        private Domain.Task GetFunctionalTask_As_InProgress()
        {
            var task = this.GetFunctionalTask_As_Planned();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            return task;
        }

        private Domain.Task GetFunctionalTask_As_Completed()
        {
            var task = this.GetFunctionalTask_As_InProgress();

            task.ChangeTaskState(TaskStateEnum.Completed);

            return task;
        }

        #endregion

        #region From Planned State

        [TestMethod]
        public void test_ChangeTaskState_From_Planned_To_Planned()
        {
            var task = this.GetFunctionalTask_As_Planned();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned &&
                 task.StartDate == null &&
                 task.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeTaskState_From_Planned_To_InProgress()
        {
            var task = this.GetFunctionalTask_As_Planned();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress &&
                 task.StartDate != null &&
                 task.FinishDate == null);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "You must informe a start date before setting a finish date")]
        public void test_ChangeTaskState_From_Planned_To_Completed()
        {
            var task = this.GetFunctionalTask_As_Planned();

            task.ChangeTaskState(TaskStateEnum.Completed);
        }

        #endregion

        #region From InProgress State

        [TestMethod]
        public void test_ChangeTaskState_From_InProgress_To_Planned()
        {
            var task = this.GetFunctionalTask_As_InProgress();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned &&
                 task.StartDate == null &&
                 task.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeTaskState_From_InProgress_To_InProgress()
        {
            var task = this.GetFunctionalTask_As_InProgress();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress &&
                task.StartDate != null &&
                task.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeTaskState_From_InProgress_To_Completed()
        {
            var task = this.GetFunctionalTask_As_InProgress();

            task.ChangeTaskState(TaskStateEnum.Completed);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Completed &&
                task.StartDate != null &&
                task.FinishDate != null);
        }

        #endregion

        #region From Completed State

        [TestMethod]
        public void test_ChangeTaskState_From_Completed_To_Planned()
        {
            var task = this.GetFunctionalTask_As_Completed();

            task.ChangeTaskState(TaskStateEnum.Planned);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Planned &&
                task.StartDate == null &&
                task.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeTaskState_From_Completed_To_InProgress()
        {
            var task = this.GetFunctionalTask_As_Completed();

            task.ChangeTaskState(TaskStateEnum.InProgress);

            Assert.IsTrue(task.TaskState == TaskStateEnum.InProgress &&
                task.StartDate != null &&
                task.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeTaskState_From_Completed_To_Completed()
        {
            var task = this.GetFunctionalTask_As_Completed();

            task.ChangeTaskState(TaskStateEnum.Completed);

            Assert.IsTrue(task.TaskState == TaskStateEnum.Completed &&
                task.StartDate != null &&
                task.FinishDate != null);
        }

        #endregion
    }
}