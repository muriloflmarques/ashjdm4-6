using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tms.Domain;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Parent Must stay as InProgress if a SubTask is InProgress

        [TestMethod]
        public void test_Parent_As_InProgress_When_SubTaskCompleted()
        {
            var parentTask = this.GetFunctionalTask();

            var childTask_1 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_1, 1);

            var subTask_1 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_1);

            parentTask.AddSubTask(subTask_1);

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent has a Child_1 in InProgress, so it should be InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            var childTask_2 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_2, 2);

            var subTask_2 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_2);

            parentTask.AddSubTask(subTask_2);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);

            //Parent has a Child_1 & Child_2 in InProgress, so it should be kept as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            //Setting Child_1 foward to Planned
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Completed);

            //Parent still has a Child_1 in InProgress, so it should be kept as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Parent_As_InProgress_When_SubTaskPlanned()
        {
            var parentTask = this.GetFunctionalTask();

            var childTask_1 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_1, 1);

            var subTask_1 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_1);

            parentTask.AddSubTask(subTask_1);

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent has a Child_1 in InProgress, so it should be InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            var childTask_2 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_2, 2);

            var subTask_2 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_2);

            parentTask.AddSubTask(subTask_2);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);

            //Parent has a Child_1 & Child_2 in InProgress, so it should be kept as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            //Setting Child_1 back to Planned
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Planned);

            //Parent still has a Child_1 in InProgress, so it should be kept as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);
        }

        #endregion

        #region Parent Must stay as Completed as long all of it's SubTask are also Completed

        [TestMethod]
        public void test_Parent_As_Completed_When_AAll_SubTasksCompleted()
        {
            var parentTask = this.GetFunctionalTask();

            var childTask_1 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_1, 1);

            var subTask_1 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_1);

            parentTask.AddSubTask(subTask_1);

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent has a Child_1 in InProgress, so it should be InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.Completed);

            //Parent has all children as Completed, so it should be Completed
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.Completed);

            var childTask_2 = this.GetFunctionalTask();

            //It's necessary to set an Id to each child so the parent can diferentiate them
            this.SetId(childTask_2, 2);

            var subTask_2 = new SubTask(
                parentTask: parentTask,
                childTask: childTask_2);

            parentTask.AddSubTask(subTask_2);

            //Parent does NOT have all children as Completed, so it should not be Completed
            Assert.IsTrue(parentTask.TaskState != TaskStateEnum.Completed);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Completed);

            //Parent has all children as Completed again, so it should be Completed
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion
    }
}