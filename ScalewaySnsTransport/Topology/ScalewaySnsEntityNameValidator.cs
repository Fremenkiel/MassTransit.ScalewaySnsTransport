namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System.Text.RegularExpressions;


    public class ScalewaySnsEntityNameValidator :
        IEntityNameValidator
    {
        static readonly Regex _regex = new Regex(@"^[A-Za-z0-9\-_\.:]+$", RegexOptions.Compiled);

        public static IEntityNameValidator Validator => Cached.EntityNameValidator;

        public void ThrowIfInvalidEntityName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ScalewaySnsTransportConfigurationException("The entity name must not be null or empty");

            var success = IsValidEntityName(name);
            if (!success)
            {
                throw new ScalewaySnsTransportConfigurationException(
                    "The entity name length must be <= 80 and a sequence of these characters: letters, digits, hyphen, underscore, period, or colon.");
            }
        }

        public bool IsValidEntityName(string name)
        {
            return _regex.Match(name).Success && name.Length <= 80;
        }


        static class Cached
        {
            internal static readonly IEntityNameValidator EntityNameValidator = new ScalewaySnsEntityNameValidator();
        }
    }
}
