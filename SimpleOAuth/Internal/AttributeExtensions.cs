// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Reflection;
using System.ComponentModel;
using SimpleOAuth.Generators;

namespace SimpleOAuth.Internal
{
    internal static class AttributeExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return (attribute == null) ? string.Empty : attribute.Description;
        }

        public static Type GetSignatureType(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            SignatureTypeAttribute attribute = Attribute.GetCustomAttribute(field, typeof(SignatureTypeAttribute)) as SignatureTypeAttribute;
            return (attribute == null) ? null : attribute.InternalType as Type;
        }
    }
}
