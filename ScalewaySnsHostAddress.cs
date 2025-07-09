namespace MassTransit
{
    using System;
    using System.Diagnostics;
    using Internals;


    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public readonly struct ScalewaySnsHostAddress
    {
        // public const string ScalewaySnsScheme = "scalewaysns";
        public const string ScalewaySnsScheme = "https";

        public readonly string Scheme;
        public readonly string Host;
        public readonly string Scope;

        public ScalewaySnsHostAddress(Uri address)
        {
            Scheme = default;
            Host = default;
            Scope = default;

            var scheme = address.Scheme.ToLowerInvariant();
            switch (scheme)
            {
                case ScalewaySnsScheme:
                    Scheme = address.Scheme;
                    Host = address.Host;

                    ParseLeft(address, out Scheme, out Host, out Scope);
                    break;

                default:
                    throw new ArgumentException($"The address scheme is not supported: {address.Scheme}", nameof(address));
            }
        }

        public ScalewaySnsHostAddress(string host, string scope)
        {
            Scheme = ScalewaySnsScheme;
            Host = host;
            Scope = scope;
        }

        static void ParseLeft(Uri address, out string scheme, out string host, out string scope)
        {
            scheme = address.Scheme;
            host = address.Host;

            scope = address.ParseHostPath();
        }

        public static implicit operator Uri(in ScalewaySnsHostAddress address)
        {
            var builder = new UriBuilder
            {
                Scheme = address.Scheme,
                Host = address.Host,
                Path = address.Scope == "/"
                    ? "/"
                    : $"/{address.Scope}"
            };

            return builder.Uri;
        }

        Uri DebuggerDisplay => this;
    }
}
