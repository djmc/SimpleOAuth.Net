// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;

namespace SimpleOAuth.Generators
{
    /// <summary>
    /// Generates a UNIX timestamp.
    /// </summary>
    public class TimestampGenerator : IGenerator
    {

        public string Generate()
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            var now = DateTime.UtcNow;

            long stamp = ((now.Ticks - unixEpoch.Ticks) / 10000000);

            return stamp.ToString();
        }

    }
}
