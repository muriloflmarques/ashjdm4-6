using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tms.Domain;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Private Methods

        public Domain.Task GetFunctionalTask_With_Two_SubTasks_All_As_Planned()
        {
            //Returns a Parent Task with Id = 1 and a Child (inside the Parent) with Id = 2
            var subTask = this.GetFunctionalSubTask();
            
            var parentTask = subTask.ParentTask;

            var childTask_2 = this.GetFunctionalTask();
            new PrivateObject(childTask_2).SetPrivateProperty("Id", 3);

            var subTask_2 = new SubTask(
                    parentTask: parentTask,
                    childTask: childTask_2);

            parentTask.AddSubTask(subTask_2);

            return parentTask;
        }

        public Domain.Task GetFunctionalTask_With_Two_SubTasks_All_As_InProgress()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_Planned();

            foreach (var subTask in parentTask.SubTasks)
                parentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.InProgress);

            return parentTask;
        }

        public Domain.Task GetFunctionalTask_With_Two_SubTasks_All_As_Completed()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_InProgress();

            foreach (var subTask in parentTask.SubTasks)
                parentTask.ChangeSubTaskState(subTask.ChildTask, TaskStateEnum.Completed);

            return parentTask;
        }

        #endregion

        #region Parent as Planned

        [TestMethod]
        public void test_Change_All_SubTaskState_To_Something_But_InProgress()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_Planned();

            var childTask_1 = parentTask.SubTasks.First().ChildTask;
            var childTask_2 = parentTask.SubTasks.Last().ChildTask;

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent must be InProgress as it has at least one child as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Completed);

            //Returning Child_1 to Planned
            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.Planned);

            //Parent Task must change to Planned once it does not have all SubTasks as Completed
            //nor have a Child as InProgress
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned &&
                childTask_1.TaskState == TaskStateEnum.Planned &&
                childTask_2.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #region Parent as InProgress

        [TestMethod]
        public void test_Change_One_SubTaskState_To_InProgress()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_Planned();

            var childTask_1 = parentTask.SubTasks.First().ChildTask;
            var childTask_2 = parentTask.SubTasks.Last().ChildTask;

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent Task must change it's State to InProgress if it has at least one
            //child as InProgress
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress &&
                childTask_1.TaskState == TaskStateEnum.InProgress &&
                childTask_2.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Change_One_SubTaskState_To_Complete_But_Keep_Another_As_InProgress()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_Planned();

            var childTask_1 = parentTask.SubTasks.First().ChildTask;
            var childTask_2 = parentTask.SubTasks.Last().ChildTask;

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Completed);

            //Parent Task must not change it's State FROM InProgress as it has at least one
            //child as InProgress
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress &&
                childTask_1.TaskState == TaskStateEnum.InProgress &&
                childTask_2.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #region Parent as Completed

        [TestMethod]
        public void test_Change_All_SubTaskState_To_Completed()
        {
            var parentTask = this.GetFunctionalTask_With_Two_SubTasks_All_As_Planned();

            var childTask_1 = parentTask.SubTasks.First().ChildTask;
            var childTask_2 = parentTask.SubTasks.Last().ChildTask;

            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.InProgress);

            //Parent must be InProgress as it has at least one child as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.InProgress);
            parentTask.ChangeSubTaskState(childTask_2, TaskStateEnum.Completed);

            //Parent must be InProgress as it has at least one child as InProgress
            Assert.IsTrue(parentTask.TaskState == TaskStateEnum.InProgress);

            //Returning Child_1 to Planned
            parentTask.ChangeSubTaskState(childTask_1, TaskStateEnum.Completed);

            //Parent Task must change to Completed once it does have all SubTasks as Completed
            //nor have a Child as InProgress
            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Completed &&
                childTask_1.TaskState == TaskStateEnum.Completed &&
                childTask_2.TaskState == TaskStateEnum.Completed);
        }

        #endregion
    }
}