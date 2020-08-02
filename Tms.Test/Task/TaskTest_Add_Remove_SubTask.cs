using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tms.Domain;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Test.Task
{
    public partial class TaskTest
    {
        #region Method AddSubTask

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Please, informe a SubTask to be added")]
        public void test_AddSubTask_Null_To_Task_With_SubTasks()
        {
            var parentTask = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            SubTask subTask = null;

            parentTask.AddSubTask(subTask);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "A SubTask can not hold any SubTask")]
        public void test_AddSubTask_Valid_To_SubTask()
        {
            var parentTask = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            var childTask_1 = this.GetFunctionalTask();

            var subTask_1 = new SubTask(
                    parentTask: parentTask,
                    childTask: childTask_1);

            parentTask.AddSubTask(subTask_1);

            var childTask_2 = this.GetFunctionalTask();

            var subTask_2 = new SubTask(
                    parentTask: childTask_1,
                    childTask: childTask_2);

            childTask_1.AddSubTask(subTask_2);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "The current SubTask belongs to another Task")]
        public void test_AddSubTask_FromAnotherParent_To_Task_With_SubTasks()
        {
            var parentTask_1 = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            var childTask = this.GetFunctionalTask();

            var subTask = new SubTask(
                    parentTask: parentTask_1,
                    childTask: childTask);

            parentTask_1.AddSubTask(subTask);

            var parentTask_2 = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            parentTask_2.AddSubTask(subTask);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Please, informe a SubTask to be added")]
        public void test_AddSubTask_Null_To_Task_Without_SubTasks()
        {
            var parentTask = this.GetFunctionalTask();

            SubTask subTask = null;

            parentTask.AddSubTask(subTask);
        }

        [TestMethod]
        public void test_AddSubTask_Valid_To_Task_With_Subtasks()
        {
            var parentTask = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            var childTask = this.GetFunctionalTask();

            var subTask = new SubTask(
                    parentTask: parentTask,
                    childTask: childTask);

            parentTask.AddSubTask(subTask);

            Assert.IsNotNull(parentTask.SubTasks.FirstOrDefault(st => subTask.Equals(st)));
        }

        [TestMethod]
        public void test_AddSubTask_Valid_To_Task_Without_Subtasks()
        {
            var parentTask = this.GetFunctionalTask();

            var childTask = this.GetFunctionalTask();

            var subTask = new SubTask(
                    parentTask: parentTask,
                    childTask: childTask);

            parentTask.AddSubTask(subTask);

            Assert.IsNotNull(parentTask.SubTasks.FirstOrDefault(st => subTask.Equals(st)));
        }


        #endregion

        #region Method RemoveSubTask

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Please, informe a SubTask to be removed")]
        public void test_RemoveSubTask_Null_From_Task_With_SubTasks()
        {
            var parentTask = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            Domain.Task task = null;

            parentTask.RemoveSubTask(task);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Please, informe a SubTask to be removed")]
        public void test_RemoveSubTask_SubTaskFromAnotherTask_From_Task_With_SubTasks()
        {
            var parentTask = this.GetFunctionalTask_WithSubTasks(subTaskAmount: 1);

            //Using a Private Object Manipulator to set propeties only acessible through bussiness logic
            new PrivateObject(parentTask.SubTasks.First().ChildTask).SetPrivateProperty("Id", 1);

            var childTask = this.GetFunctionalTask();
            //The current ChildTask in SubTasks of ParentTask is 1
            //So, by setting this child another Id the ParentTask must be able to see that it does not belong to it
            new PrivateObject(childTask).SetPrivateProperty("Id", 2);

            parentTask.RemoveSubTask(childTask);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Please, informe a SubTask to be removed")]
        public void test_RemoveSubTask_Null_From_Task_Without_SubTasks()
        {
            var parentTask = this.GetFunctionalTask();

            Domain.Task task = null;

            parentTask.RemoveSubTask(task);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "The current Task has no SubTasks")]
        public void test_RemoveSubTask_SubTaskFromAnotherTask_From_Task_Without_SubTasks()
        {
            var parentTask = this.GetFunctionalTask();

            var childTask = this.GetFunctionalTask();

            parentTask.RemoveSubTask(childTask);
        }

        #endregion

        #region Check Parent's Status when adding SubTask

        #region Adding SubTask To a Planned Parent

        [TestMethod]
        public void test_Adding_PlannedSubTask_To_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should not change it's state nor throw a exception once it's already Planned

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Adding_InProgressSubTask_To_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should follow the SubTask into InProgress Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Adding_CompletedSubTask_To_PlannedParent()
        {
            var parentTask = this.GetFunctionalTask_With_PlannedStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should follow the SubTask into Completed Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
               parentTask.TaskState == TaskStateEnum.Completed
               && childTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #region Adding SubTask To a InProgress Parent

        [TestMethod]
        public void test_Adding_PlannedSubTask_To_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should not be kept as InProgress because it does not have a child in InProgress Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Adding_InProgressSubTask_To_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should not change it's state nor throw a exception once it's already InProgress

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Adding_CompletedSubTask_To_InProgressParent()
        {
            var parentTask = this.GetFunctionalTask_With_InProgressStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should follow the SubTask into Completed Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
               parentTask.TaskState == TaskStateEnum.Completed
               && childTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #region Adding SubTask To a Complete Parent

        [TestMethod]
        public void test_Adding_PlannedSubTask_To_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_PlannedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should not be kept as Completes because it does not have all children in Complete Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.Planned
                && childTask.TaskState == TaskStateEnum.Planned);
        }

        [TestMethod]
        public void test_Adding_InProgressSubTask_To_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_InProgressStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should not be kept as Completes because it does not have all children in Complete Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
                parentTask.TaskState == TaskStateEnum.InProgress
                && childTask.TaskState == TaskStateEnum.InProgress);
        }

        [TestMethod]
        public void test_Adding_CompletedSubTask_To_CompleteParent()
        {
            var parentTask = this.GetFunctionalTask_With_CompletedStatus();
            var childTask = this.GetFunctionalTask_With_CompletedStatus();

            var subTask = new SubTask(
                parentTask: parentTask,
                childTask: childTask);

            //It should be kept as Completes because it does have all children in Complete Status

            parentTask.AddSubTask(subTask);

            Assert.IsTrue(
               parentTask.TaskState == TaskStateEnum.Completed
               && childTask.TaskState == TaskStateEnum.Completed);
        }

        #endregion

        #endregion
    }
}