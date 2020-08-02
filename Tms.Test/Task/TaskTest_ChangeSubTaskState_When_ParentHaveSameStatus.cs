using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tms.Domain;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Check Parent's Status when changing SubTask's Status

        #region Changing Status of a Planned SubTask of a Planned Parent

        [TestMethod]
        public void test_Changing_PlannedSubTask_To_Planned_In_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Planned);

            //It should not change the SubTask's Status because it is already Planned and no change
            //does not trigger the ParentTask's Status to change
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Changing_PlannedSubTask_To_InProgress_In_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.InProgress);

            //It should change the ParentTask's Status because it has a child in InProgress Status

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainRulesException))]
        public void test_Changing_PlannedSubTask_To_Complete_In_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            //No SubTask can go from Planned to Complete without steping in the InProgress Status
            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Completed);
        }

        #endregion

        #region Changing Status of a InProgress SubTask of a InProgress Parent

        [TestMethod]
        public void test_Changing_InProgressSubTask_To_Planned_In_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Planned);

            //It should change the Parent's to Planned because no Child is in InProgress

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Changing_InProgressSubTask_To_InProgress_In_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.InProgress);

            //It should not change the Parent's because no trigger was activated by changing a Child Status

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Changing_InProgressSubTask_To_Complete_In_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Completed);

            //It should change the ParentTask's Status to Completed because all children are in Complete Status

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Completed
                && childTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #region Changing Status of a InProgress SubTask of a InProgress Parent

        [TestMethod]
        public void test_Changing_CompleteSubTask_To_Planned_In_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Planned);

            //It should change the Parent's to Planned because no Child is in InProgress

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Changing_CompleteSubTask_To_InProgress_In_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.InProgress);

            //It should change the Parent's to InProgress because it ha a Child in InProgress

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Changing_CompleteSubTask_To_Complete_In_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            parentTask.AddSubTask(subTask);

            parentTask.ChangeSubTaskState(childTask, TaskStateEnum.Completed);

            //It should not change the SubTask's Status because it is already Completed and no change
            //does not trigger the ParentTask's Status to change
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Completed
                && childTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #endregion
    }
}