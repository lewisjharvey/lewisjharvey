using System;
using System.Collections.Generic;
using System.Text;

namespace Greenvale.ActiveDirectory.Intextra
{
    public class DirectoryEntryAttribute
    {
        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public AttributeType AttributeType
        {
            get;
            set;
        }

        public DirectoryEntryAttribute(string name, string value, AttributeType attributeType)
        {
            this.Name = name;
            this.Value = value;
            this.AttributeType = attributeType;
        }

        public override string ToString()
        {
            return string.Format("Name: {0} | Value: {1} | Attribute Type: {3}", this.Name, this.Value, this.AttributeType);
        }
    }
}
