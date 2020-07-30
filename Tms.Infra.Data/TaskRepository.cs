using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class TaskRepository : BaseRepository<Domain.Task>, ITaskRepository { }
}