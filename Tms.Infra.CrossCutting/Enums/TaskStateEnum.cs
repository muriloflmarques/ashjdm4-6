namespace Tms.Infra.CrossCutting.Enums
{
    /// <summary>
    /// Enums are comonly stored in the Domain, but since the Domain is usualy
    /// not accessible to the Application then I decided to store Enums in the
    /// Cross Cutting, if it seems inapropriate it's possible to create a DTO
    /// to send Enum information to the Application
    /// </summary>
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