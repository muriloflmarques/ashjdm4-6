using System;

namespace Tms.Infra.CrossCutting
{
    /// <summary>
    /// Attribute to help to display Enums in a more fashionable way
    /// </summary>
    public class EnumValueAsText: Attribute
    {
        public EnumValueAsText(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}