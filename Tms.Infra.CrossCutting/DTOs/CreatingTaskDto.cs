namespace Tms.Infra.CrossCutting.DTOs
{
    //All DTOs are structs, it ensures that after populated the data will be always immutable
    public struct CreatingTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentTaskId { get; set; }
    }
}