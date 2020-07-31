namespace Tms.Domain
{
    public class SubTask
    {
        protected SubTask() { }

        public SubTask(Task parentTask, Task task)
        {
            this.ParentTask = parentTask;
            this.ChildTask = task;
        }

        public int? ParentTaskId { get; private set; }
        public Task ParentTask { get; private set; }

        public int? ChildTaskId { get; private set; }
        public Task ChildTask { get; private set; }
    }
}