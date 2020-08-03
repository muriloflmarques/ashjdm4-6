using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tms.Domain;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Private Methods

        public SubTask GetFunctionalSubTask_As_Planned() =>
            this.GetFunctionalSubTask();

        public SubTask GetFunctionalSubTask_As_InProgress()
        {
            var subTask = this.GetFunctionalSubTask_As_Planned();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.InProgress);

            return subTask;
        }

        public SubTask GetFunctionalSubTask_As_Completed()
        {
            var subTask = this.GetFunctionalSubTask_As_InProgress();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);

            return subTask;
        }

        #endregion

        #region From Planned State

        [TestMethod]
        public void test_ChangeSubTaskState_From_Planned_To_Planned()
        {
            var subTask = this.GetFunctionalSubTask_As_Planned();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Planned);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.Planned &&
                subTask.ParentTask.StartDate == null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Planned &&
                subTask.ChildTask.StartDate == null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_From_Planned_To_InProgress()
        {
            var subTask = this.GetFunctionalSubTask_As_Planned();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.InProgress);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ParentTask.StartDate != null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ChildTask.StartDate != null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException), "You must informe a start date before setting a finish date")]
        public void test_ChangeSubTaskState_From_Planned_To_Completed()
        {
            var subTask = this.GetFunctionalSubTask_As_Planned();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);
        }

        #endregion

        #region From InProgress State

        [TestMethod]
        public void test_ChangeSubTaskState_From_InProgress_To_Planned()
        {
            var subTask = this.GetFunctionalSubTask_As_InProgress();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Planned);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.Planned &&
                subTask.ParentTask.StartDate == null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Planned &&
                subTask.ChildTask.StartDate == null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_From_InProgress_To_InProgress()
        {
            var subTask = this.GetFunctionalSubTask_As_InProgress();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.InProgress);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ParentTask.StartDate != null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ChildTask.StartDate != null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_From_InProgress_To_Completed()
        {
            var subTask = this.GetFunctionalSubTask_As_InProgress();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.Completed &&
                subTask.ParentTask.StartDate != null &&
                subTask.ParentTask.FinishDate != null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Completed &&
                subTask.ChildTask.StartDate != null &&
                subTask.ChildTask.FinishDate != null);
        }

        #endregion

        #region From Complete State

        [TestMethod]
        public void test_ChangeSubTaskState_From_Completed_To_Planned()
        {
            var subTask = this.GetFunctionalSubTask_As_Completed();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Planned);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.Planned &&
                subTask.ParentTask.StartDate == null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Planned &&
                subTask.ChildTask.StartDate == null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_From_Completed_To_InProgress()
        {
            var subTask = this.GetFunctionalSubTask_As_Completed();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.InProgress);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ParentTask.StartDate != null &&
                subTask.ParentTask.FinishDate == null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.InProgress &&
                subTask.ChildTask.StartDate != null &&
                subTask.ChildTask.FinishDate == null);
        }

        [TestMethod]
        public void test_ChangeSubTaskState_From_Completed_To_Completed()
        {
            var subTask = this.GetFunctionalSubTask_As_Completed();

            subTask.ParentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);

            Assert.IsTrue(subTask.ParentTask.TaskState == TaskStateEnum.Completed &&
                subTask.ParentTask.StartDate != null &&
                subTask.ParentTask.FinishDate != null);

            Assert.IsTrue(subTask.ChildTask.TaskState == TaskStateEnum.Completed &&
                subTask.ChildTask.StartDate != null &&
                subTask.ChildTask.FinishDate != null);
        }

        #endregion
    }
}