namespace Tms.Infra.Data.Interface
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        void AddRawCommand(RawSQlCommand rawSQlCommand);
    }
}