using System;

namespace Congress.Core.Tags
{
    public class EnumValue : Attribute
    {
        public string value { get; set; }
        public EnumValue(string value)
        {
            this.value = value;
        }
    }
}
