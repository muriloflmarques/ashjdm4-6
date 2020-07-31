namespace Tms.Infra.Data.Interface
{
    public interface ITaskRepository : IBaseRepository<Domain.Task> 
    {
        Domain.Task FindParentTask(int subTaskId);
    }
}