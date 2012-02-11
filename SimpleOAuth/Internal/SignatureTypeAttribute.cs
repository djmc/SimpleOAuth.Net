// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;

namespace SimpleOAuth.Internal
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class SignatureTypeAttribute : Attribute
    {
        readonly Type _internalType;

        public SignatureTypeAttribute(Type internalType)
        {
            this._internalType = internalType;
        }

        public Type InternalType
        {
            get { return _internalType; }
        }
    }
}
