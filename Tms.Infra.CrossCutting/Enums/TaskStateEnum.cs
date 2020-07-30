namespace Tms.Infra.CrossCutting.Enums
{
    public enum TaskStateEnum
    {
        [EnumValueAsText("Planned")]
        Planned = 0,

        [EnumValueAsText("In Progress")]
        InProgress = 5,

        [EnumValueAsText("Completed")]
        Completed = 10
    }
}