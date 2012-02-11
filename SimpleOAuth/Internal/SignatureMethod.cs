// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Collections.Generic;
using SimpleOAuth.Generators;

namespace SimpleOAuth.Internal
{
    internal static class SignatureMethod
    {

        private static Dictionary<EncryptionMethod, Generators.ISignatureGenerator> _instancedGenerators = new Dictionary<EncryptionMethod, ISignatureGenerator>();

        private static ISignatureGenerator GetGeneratorForMethod(EncryptionMethod signMethod)
        {
            if (_instancedGenerators.ContainsKey(signMethod))
            {
                return _instancedGenerators[signMethod];
            }
            else
            {
                Type signType = signMethod.GetSignatureType();
                ISignatureGenerator instancedType = Activator.CreateInstance(signType) as ISignatureGenerator;
                _instancedGenerators.Add(signMethod, instancedType);
                return instancedType;
            }
        }

        public static string CreateSignature(EncryptionMethod signMethod, string baseString, string key)
        {
            return GetGeneratorForMethod(signMethod).Generate(baseString.Trim(), key.Trim());
        }

    }
}
