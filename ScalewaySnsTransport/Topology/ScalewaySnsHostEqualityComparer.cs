namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;


    public sealed class ScalewaySnsHostEqualityComparer :
        IEqualityComparer<ScalewaySnsHostSettings>
    {
        public static IEqualityComparer<ScalewaySnsHostSettings> Default { get; } = new ScalewaySnsHostEqualityComparer();

        public bool Equals(ScalewaySnsHostSettings x, ScalewaySnsHostSettings y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null))
                return false;

            if (ReferenceEquals(y, null))
                return false;

            return string.Equals(x.Region.SystemName, y.Region.SystemName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ScalewaySnsHostSettings obj)
        {
            unchecked
            {
                var hashCode = 0;
                if (!string.IsNullOrEmpty(obj.SqsAccessKey))
                {
                    hashCode = obj.SqsAccessKey?.GetHashCode() ?? 0;
                }

                if(!string.IsNullOrEmpty(obj.SnsAccessKey))
                {
                    hashCode = obj.SnsAccessKey?.GetHashCode() ?? 0;
                }

                hashCode = (hashCode * 397) ^ (obj.Region?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
