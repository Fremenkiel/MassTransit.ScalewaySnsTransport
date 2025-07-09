/*
namespace MassTransit.ScalewaySnsTransport.Configuration;

using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Endpoints;
using Amazon.Runtime.Internal;
using Amazon.SQS;
using Amazon.SQS.Internal;
using Amazon.Util.Internal;

public partial class ScalewaySQSConfig : ScalewayClientConfig
    {
        private static readonly string UserAgentString =
            InternalSDKUtils.BuildUserAgentString("SQS", "4.0.0.0");

        private static readonly AmazonSQSEndpointResolver EndpointResolver =
            new AmazonSQSEndpointResolver();

        private string _userAgent = UserAgentString;
        ///<summary>
        /// The ServiceId, which is the unique identifier for a service.
        ///</summary>
        public static new string ServiceId
        {
            get
            {
                return "SQS";
            }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScalewaySQSConfig()
            : base(new Amazon.Runtime.Internal.DefaultConfigurationProvider(AmazonSQSDefaultConfiguration.GetAllConfigurations()))
        {
            base.ServiceId = "SQS";
            this.AuthenticationServiceName = "sqs";
            this.EndpointProvider = new AmazonSQSEndpointProvider();
        }

        /// <summary>
        /// The constant used to lookup in the region hash the endpoint.
        /// </summary>
        public override string RegionEndpointServiceName
        {
            get
            {
                return "sqs";
            }
        }

        /// <summary>
        /// Gets the ServiceVersion property.
        /// </summary>
        public override string ServiceVersion
        {
            get
            {
                return "2012-11-05";
            }
        }

        /// <summary>
        /// Gets the value of UserAgent property.
        /// </summary>
        public override string UserAgent
        {
            get
            {
                return _userAgent;
            }
        }

        /// <summary>
        /// Returns the endpoint that will be used for a particular request.
        /// </summary>
        /// <param name="parameters">A Container class for parameters used for endpoint resolution.</param>
        /// <returns>The resolved endpoint for the given request.</returns>
        public override Amazon.Runtime.Endpoints.Endpoint DetermineServiceOperationEndpoint(ServiceOperationEndpointParameters parameters)
        {
            var requestContext = new RequestContext(false)
            {
                ClientConfig = this,
                OriginalRequest = parameters.Request,
                Request = new DefaultRequest(parameters.Request, ServiceId)
                {
                    AlternateEndpoint = parameters.AlternateEndpoint
                }
            };

            var executionContext = new Amazon.Runtime.Internal.ExecutionContext(requestContext, null);
            return EndpointResolver.GetEndpoint(executionContext);
        }

}
*/
