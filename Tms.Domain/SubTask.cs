namespace Tms.Domain
{
    public class SubTask
    {
        protected SubTask() { }

        public SubTask(Task parentTask, Task childTask)
        {
            this.ParentTask = parentTask;
            this.ChildTask = childTask;
        }

        public int? ParentTaskId { get; private set; }
        public Task ParentTask { get; private set; }

        public int? ChildTaskId { get; private set; }
        public Task ChildTask { get; private set; }
    }
}