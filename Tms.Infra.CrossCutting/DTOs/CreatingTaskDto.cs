namespace Tms.Infra.CrossCutting.DTOs
{
    /// <summary>
    /// All DTOs are structs, it ensures that after populated the data will be always immutable
    /// </summary>
    public struct CreatingTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentTaskId { get; set; }
    }
}