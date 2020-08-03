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
        //I usualy leave "empty numbers" between enums so if the domain changes and a new value addition is needed
        //then the logical evolution can be follwed by the numeric evolution, making it easy to see in any log
        [EnumValueAsText("Planned")]
        Planned = 0,

        [EnumValueAsText("In Progress")]
        InProgress = 5,

        [EnumValueAsText("Completed")]
        Completed = 10
    }
}