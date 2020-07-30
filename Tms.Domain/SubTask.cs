namespace Tms.Domain
{
    public class SubTask
    {
        protected SubTask() { }

        public SubTask(Task parentTask, Task task)
        {
            this.ParentTask = parentTask;
            this.Task = task;
        }

        public int ParentTaskId { get; private set; }
        public Task ParentTask { get; private set; }

        public int TaskId { get; private set; }
        public Task Task { get; private set; }
    }
}