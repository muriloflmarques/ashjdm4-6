namespace Tms.Infra.CrossCutting.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this System.Enum enumVal)
        {
            var type = enumVal?.GetType();
            var memInfo = type?.GetMember(enumVal?.ToString());
            var attributes = memInfo[0]?.GetCustomAttributes(typeof(EnumValueAsText), false);

            return (attributes?.Length > 0) ? ((EnumValueAsText)attributes[0])?.Value : null;
        }
    }
}