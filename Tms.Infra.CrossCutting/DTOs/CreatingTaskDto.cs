namespace Tms.Infra.CrossCutting.DTOs
{
    public struct CreatingTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentTaskId { get; set; }
    }
}