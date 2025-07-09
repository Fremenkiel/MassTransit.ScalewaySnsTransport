/*
namespace MassTransit.ScalewaySnsTransport.Configuration;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Globalization;
using Amazon;
using Amazon.Runtime;
using Amazon.Util;
using Amazon.Runtime.Endpoints;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal.Settings;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Credentials.Internal;
using Amazon.Runtime.Identity;
using Amazon.Runtime.Credentials;



#if NETSTANDARD
using System.Runtime.InteropServices;
#endif


    public abstract partial class ScalewayClientConfig : IClientConfig
    {
        internal static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);
        internal const long UpperLimitCompressionSizeBytes = 10485760;

        public static readonly TimeSpan MaxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private IDefaultConfigurationProvider _defaultConfigurationProvider;
        private string serviceId = null;
        private DefaultConfigurationMode? defaultConfigurationMode;
        private RegionEndpoint regionEndpoint = null;
        private bool probeForRegionEndpoint = true;
        private bool throttleRetries = true;
        private bool useHttp = false;
        private bool useAlternateUserAgentHeader = AWSConfigs.UseAlternateUserAgentHeader;
        private string serviceURL = null;
        private string authRegion = null;
        private string authServiceName = null;
        private string clientAppId = null;
        private SigningAlgorithm signatureMethod = SigningAlgorithm.HmacSHA256;
        private bool logResponse = false;
        private int bufferSize = AWSSDKUtils.DefaultBufferSize;
        private long progressUpdateInterval = AWSSDKUtils.DefaultProgressUpdateInterval;
        private bool resignRetries = false;
        private ICredentials proxyCredentials;
        private bool logMetrics = AWSConfigs.LoggingConfig.LogMetrics;
        private bool disableLogging = false;
        private TimeSpan? timeout = null;
        private bool allowAutoRedirect = true;
        private bool? useDualstackEndpoint;
        private bool? useFIPSEndpoint;
        private bool? disableRequestCompression;
        private long? requestMinCompressionSizeBytes;
        private bool disableHostPrefixInjection = false;
        private bool? endpointDiscoveryEnabled = null;
        private bool? ignoreConfiguredEndpointUrls;
        private int endpointDiscoveryCacheLimit = 1000;
        private RequestRetryMode? retryMode = null;
        private int? maxRetries = null;
        private const int MaxRetriesDefault = 2;
        private const long DefaultMinCompressionSizeBytes = 10240;
        private bool didProcessServiceURL = false;
        private AWSCredentials _defaultAWSCredentials = null;
        private IIdentityResolverConfiguration _identityResolverConfiguration = DefaultIdentityResolverConfiguration.Instance;
        private IAWSTokenProvider _awsTokenProvider;
        private TelemetryProvider telemetryProvider = AWSConfigs.TelemetryProvider;
        private AccountIdEndpointMode? accountIdEndpointMode = null;
        private RequestChecksumCalculation? requestChecksumCalculation = null;
        private ResponseChecksumValidation? responseChecksumValidation = null;

        private CredentialProfileStoreChain credentialProfileStoreChain;
#if BCL
        private readonly TcpKeepAlive tcpKeepAlive = new TcpKeepAlive();
#endif
        public AccountIdEndpointMode AccountIdEndpointMode
        {
            get
            {
                if (!accountIdEndpointMode.HasValue)
                {
                    return FallbackInternalConfigurationFactory.AccountIdEndpointMode ?? AccountIdEndpointMode.PREFERRED;
                }
                return accountIdEndpointMode.Value;
            }
            set
            {
                this.accountIdEndpointMode = value;
            }
        }
        public Profile Profile { get; set; }
        private CredentialProfileStoreChain CredentialProfileStoreChain
        {
            get
            {
                if (credentialProfileStoreChain == null)
                {
                    if(Profile != null)
                    {
                        credentialProfileStoreChain = new CredentialProfileStoreChain(Profile.Location);
                    }
                    else
                    {
                        credentialProfileStoreChain = new CredentialProfileStoreChain();
                    }

                }
                return credentialProfileStoreChain;
            }
            set
            {
                credentialProfileStoreChain = value;
            }
        }

#if BCL
        private static WebProxy GetWebProxyWithCredentials(string value)
#else
        private static Amazon.Runtime.Internal.Util.WebProxy GetWebProxyWithCredentials(string value)
#endif
        {
            if (!string.IsNullOrEmpty(value))
            {
#if BCL
                if (!value.Contains("://"))
                {
                    value = "http://" + value;
                }
                var asUri = new Uri(value);

                var parsedProxy = new WebProxy(asUri);
#else
                var asUri = new Uri(value);
                var parsedProxy = new Amazon.Runtime.Internal.Util.WebProxy(asUri);
#endif
                if (!string.IsNullOrEmpty(asUri.UserInfo)) {
                    var userAndPass = asUri.UserInfo.Split(':');
                    parsedProxy.Credentials = new NetworkCredential(
                        userAndPass[0],
                        userAndPass.Length > 1 ? userAndPass[1] : string.Empty
                    );
                }
                return parsedProxy;
            }

            return null;
        }

        public AWSCredentials DefaultAWSCredentials
        {
            get { return this._defaultAWSCredentials; }
            set { this._defaultAWSCredentials = value; }
        }

        public IIdentityResolverConfiguration IdentityResolverConfiguration
        {
            get { return this._identityResolverConfiguration; }
            set { this._identityResolverConfiguration = value; }
        }

        public IAWSTokenProvider AWSTokenProvider
        {
            get { return this._awsTokenProvider; }
            set { this._awsTokenProvider = value; }
        }

        public abstract string ServiceVersion
        {
            get;
        }

        public SigningAlgorithm SignatureMethod
        {
            get { return this.signatureMethod; }
            set { this.signatureMethod = value; }
        }

        public abstract string UserAgent { get; }

        public bool UseAlternateUserAgentHeader
        {
            get { return this.useAlternateUserAgentHeader; }
            set { this.useAlternateUserAgentHeader = value; }
        }

        public RegionEndpoint RegionEndpoint
        {
            get
            {
                if (probeForRegionEndpoint)
                {
                    RegionEndpoint = GetDefaultRegionEndpoint();
                    this.probeForRegionEndpoint = false;
                }
                return this.regionEndpoint;
            }
            set
            {
                if (!string.IsNullOrEmpty(this.serviceURL))
                    Logger.GetLogger(GetType()).InfoFormat($"RegionEndpoint and ServiceURL are mutually exclusive. Since " +
                        $"RegionEndpoint was set last, RegionEndpoint: {value} will be used to make the request and ServiceUrl: {this.serviceURL} has been set to null.");
                this.defaultConfigurationBackingField = null;
                this.serviceURL = null;
                this.regionEndpoint = value;
                this.probeForRegionEndpoint = this.regionEndpoint == null;

                if (!string.IsNullOrEmpty(value?.SystemName) &&
                    (value.SystemName.Contains("fips-") || value.SystemName.Contains("-fips")))
                {
                    Logger.GetLogger(GetType()).InfoFormat($"FIPS Pseudo Region support is deprecated. Will attempt to convert {value.SystemName}.");

                    this.UseFIPSEndpoint = true;
                    this.regionEndpoint =
                        RegionEndpoint.GetBySystemName(
                            value.SystemName.Replace("fips-", "").Replace("-fips", ""));
                }
            }
        }

        public abstract string RegionEndpointServiceName
        {
            get;
        }

        public string ServiceURL
        {
            get
            {
                if (!didProcessServiceURL && this.serviceURL == null && IgnoreConfiguredEndpointUrls == false && ServiceId != null)
                {

                    string serviceSpecificTransformedEnvironmentVariable = TransformServiceId.TransformServiceIdToEnvVariable(ServiceId);
                    string transformedConfigServiceId = TransformServiceId.TransformServiceIdToConfigVariable(ServiceId);

                    if (Environment.GetEnvironmentVariable(serviceSpecificTransformedEnvironmentVariable) != null)
                    {
                        Logger.GetLogger(GetType()).InfoFormat($"ServiceURL configured from service specific environment variable: {serviceSpecificTransformedEnvironmentVariable}.");
                        this.ServiceURL = Environment.GetEnvironmentVariable(serviceSpecificTransformedEnvironmentVariable);
                    }
                    else if (Environment.GetEnvironmentVariable(EnvironmentVariables.GLOBAL_ENDPOINT_ENVIRONMENT_VARIABLE) != null)
                    {
                        this.ServiceURL = Environment.GetEnvironmentVariable(EnvironmentVariables.GLOBAL_ENDPOINT_ENVIRONMENT_VARIABLE);
                        Logger.GetLogger(GetType()).InfoFormat($"ServiceURL configured from global environment variable: {EnvironmentVariables.GLOBAL_ENDPOINT_ENVIRONMENT_VARIABLE}.");
                    }
                    didProcessServiceURL = true;
                }
                return this.serviceURL;

            }
            set
            {
                if (regionEndpoint != null)
                    Logger.GetLogger(GetType()).InfoFormat($"RegionEndpoint and ServiceURL are mutually exclusive. Since " +
                        $"ServiceUrl was set last, ServiceUrl: {value} will be used to make the request and RegionEndpoint: {this.regionEndpoint} has been set to null.");
                this.regionEndpoint = null;
                this.probeForRegionEndpoint = false;

                if(!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        var path = new Uri(value).PathAndQuery;
                        if (string.IsNullOrEmpty(path) || path == "/")
                        {
                            if (!string.IsNullOrEmpty(value) && !value.EndsWith("/"))
                            {
                                value += "/";
                            }
                        }
                    }
                    catch(UriFormatException)
                    {
                        throw new AmazonClientException("Value for ServiceURL is not a valid URL: " + value);
                    }
                }

                this.serviceURL = value;
            }
        }

        public bool UseHttp
        {
            get { return this.useHttp; }
            set { this.useHttp = value; }
        }

        public string AuthenticationRegion
        {
            get { return this.authRegion; }
            set { this.authRegion = value; }
        }

        public string AuthenticationServiceName
        {
            get { return this.authServiceName; }
            set { this.authServiceName = value; }
        }

        public string ServiceId
        {
            get { return this.serviceId; }
            set { this.serviceId = value; }
        }

        public int MaxErrorRetry
        {
            get
            {
                if (!this.maxRetries.HasValue)
                {
                    return FallbackInternalConfigurationFactory.MaxAttempts - 1 ?? MaxRetriesDefault;
                }

                return this.maxRetries.Value;
            }
            set { this.maxRetries = value; }
        }

        public bool IsMaxErrorRetrySet
        {
            get
            {
                return this.maxRetries.HasValue;
            }
        }

        public bool LogResponse
        {
            get { return this.logResponse; }
            set { this.logResponse = value; }
        }

        public int BufferSize
        {
            get { return this.bufferSize; }
            set { this.bufferSize = value; }
        }

        public long ProgressUpdateInterval
        {
            get { return progressUpdateInterval; }
            set { progressUpdateInterval = value; }
        }

        public bool ResignRetries
        {
            get { return this.resignRetries; }
            set { this.resignRetries = value; }
        }

        public bool AllowAutoRedirect
        {
            get
            {
                return this.allowAutoRedirect;
            }
            set
            {
                this.allowAutoRedirect = value;
            }
        }

        public bool LogMetrics
        {
            get { return this.logMetrics; }
            set { this.logMetrics = value; }
        }

        public bool DisableLogging
        {
            get { return this.disableLogging; }
            set { this.disableLogging = value; }
        }

        public DefaultConfigurationMode DefaultConfigurationMode
        {
            get
            {
                if (this.defaultConfigurationMode.HasValue)
                    return this.defaultConfigurationMode.Value;

                return DefaultConfiguration.Name;
            }
            set
            {
                this.defaultConfigurationMode = value;
                defaultConfigurationBackingField = null;
            }
        }

        private IDefaultConfiguration defaultConfigurationBackingField;
        protected IDefaultConfiguration DefaultConfiguration
        {
            get
            {
                if (defaultConfigurationBackingField != null)
                    return defaultConfigurationBackingField;

                defaultConfigurationBackingField =
                    _defaultConfigurationProvider.GetDefaultConfiguration(RegionEndpoint, defaultConfigurationMode);

                return defaultConfigurationBackingField;
            }
        }

        public ICredentials ProxyCredentials
        {
            get
            {
                if(this.proxyCredentials == null &&
                    (!string.IsNullOrEmpty(AWSConfigs.ProxyConfig.Username) ||
                    !string.IsNullOrEmpty(AWSConfigs.ProxyConfig.Password)))
                {
                    return new NetworkCredential(AWSConfigs.ProxyConfig.Username, AWSConfigs.ProxyConfig.Password ?? string.Empty);
                }
                return this.proxyCredentials;
            }
            set { this.proxyCredentials = value; }
        }

#if BCL
        public WebProxy GetHttpProxy()
#else
        public IWebProxy GetHttpProxy()
#endif
        {
            var explicitProxy = GetWebProxy();
            if (explicitProxy != null)
            {
                return explicitProxy;
            }
            return GetWebProxyWithCredentials(Environment.GetEnvironmentVariable("http_proxy"));
        }

#if BCL
        public WebProxy GetHttpsProxy()
#else
        public IWebProxy GetHttpsProxy()
#endif
        {
            var explicitProxy = GetWebProxy();
            if (explicitProxy != null)
            {
                return explicitProxy;
            }
            return GetWebProxyWithCredentials(Environment.GetEnvironmentVariable("https_proxy"));
        }

#if BCL
        public TcpKeepAlive TcpKeepAlive
        {
            get { return this.tcpKeepAlive; }
        }
#endif

        #region Constructor

        #endregion

        protected virtual void Initialize()
        {
        }

        public TimeSpan? Timeout
        {
            get
            {
                if (this.timeout.HasValue)
                    return this.timeout;

                return DefaultConfiguration.TimeToFirstByteTimeout;
            }
            set
            {
                ValidateTimeout(value);
                this.timeout = value;
            }
        }

#if AWS_ASYNC_API
        internal CancellationToken BuildDefaultCancellationToken()
        {
            // TimeToFirstByteTimeout is not a perfect match with HttpWebRequest/HttpClient.Timeout.  However, given
            // that both are configured to only use Timeout until the Response Headers are downloaded, this value
            // provides a reasonable default value.
            var cancelTimeout = DefaultConfiguration.TimeToFirstByteTimeout;

            return cancelTimeout.HasValue
                ? new CancellationTokenSource(cancelTimeout.Value).Token
                : default(CancellationToken);
        }
#endif

        public bool UseDualstackEndpoint
        {
            get
            {
                if (!this.useDualstackEndpoint.HasValue)
                {
                    return FallbackInternalConfigurationFactory.UseDualStackEndpoint ?? false;
                }

                return this.useDualstackEndpoint.Value;
            }
            set { useDualstackEndpoint = value; }
        }

        public bool UseFIPSEndpoint
        {
            get
            {
                if (!this.useFIPSEndpoint.HasValue)
                {
                    return FallbackInternalConfigurationFactory.UseFIPSEndpoint ?? false;
                }

                return this.useFIPSEndpoint.Value;
            }
            set { useFIPSEndpoint = value; }
        }

        public bool IgnoreConfiguredEndpointUrls
        {
            get
            {
                if (!this.ignoreConfiguredEndpointUrls.HasValue)
                    return FallbackInternalConfigurationFactory.IgnoreConfiguredEndpointUrls ?? false;

                return this.ignoreConfiguredEndpointUrls.Value;
            }
            set { ignoreConfiguredEndpointUrls = value; }
        }

        public bool DisableRequestCompression
        {
            get
            {
                if (!this.disableRequestCompression.HasValue)
                {
                    return FallbackInternalConfigurationFactory.DisableRequestCompression ?? false;
                }

                return this.disableRequestCompression.Value;
            }
            set { disableRequestCompression = value; }
        }

        public long RequestMinCompressionSizeBytes
        {
            get
            {
                if (!this.requestMinCompressionSizeBytes.HasValue)
                {
                    return FallbackInternalConfigurationFactory.RequestMinCompressionSizeBytes ?? DefaultMinCompressionSizeBytes;
                }

                return this.requestMinCompressionSizeBytes.Value;
            }
            set
            {
                ValidateMinCompression(value);
                requestMinCompressionSizeBytes = value;
            }
        }

        public string ClientAppId
        {
            get
            {
                if (this.clientAppId == null)
                {
                    return FallbackInternalConfigurationFactory.ClientAppId;
                }

                return this.clientAppId;
            }
            set
            {
                ValidateClientAppId(value);
                this.clientAppId = value;
            }
        }

        private static void ValidateClientAppId(string clientAppId)
        {
            if (clientAppId != null && clientAppId.Length > EnvironmentVariableInternalConfiguration.AWS_SDK_UA_APP_ID_MAX_LENGTH)
            {
                Logger.GetLogger(typeof(InternalConfiguration)).InfoFormat("Warning: Client app id exceeds recommended maximum length of {0} characters: \"{1}\"", EnvironmentVariableInternalConfiguration.AWS_SDK_UA_APP_ID_MAX_LENGTH, clientAppId);
            }
        }

        private static void ValidateMinCompression(long minCompressionSize)
        {
            if (minCompressionSize < 0 || minCompressionSize > UpperLimitCompressionSizeBytes)
            {
                throw new ArgumentException(string.Format("Invalid value {0} for {1}." +
                    " A long value between 0 and {2} bytes inclusive is expected.", minCompressionSize,
                    nameof(requestMinCompressionSizeBytes), UpperLimitCompressionSizeBytes));
            }
        }

        public bool ThrottleRetries
        {
            get { return throttleRetries; }
            set { throttleRetries = value; }
        }

        public void SetUseNagleIfAvailable(bool useNagle)
        {
#if BCL
            this.UseNagleAlgorithm = useNagle;
#endif
        }

        public virtual void Validate()
        {
            if (RegionEndpoint == null && string.IsNullOrEmpty(this.ServiceURL))
                throw new AmazonClientException("No RegionEndpoint or ServiceURL configured");
#if BCL
            if (TcpKeepAlive.Enabled)
            {
                ValidateTcpKeepAliveTimeSpan(TcpKeepAlive.Timeout, "TcpKeepAlive.Timeout");
                ValidateTcpKeepAliveTimeSpan(TcpKeepAlive.Interval, "TcpKeepAlive.Interval");
            }
#endif
        }

        public bool DisableHostPrefixInjection
        {
            get { return this.disableHostPrefixInjection; }
            set { this.disableHostPrefixInjection = value; }
        }

        public bool EndpointDiscoveryEnabled
        {
            get
            {
                if (!this.endpointDiscoveryEnabled.HasValue)
                {
                    return FallbackInternalConfigurationFactory.EndpointDiscoveryEnabled ?? false;
                }

                return this.endpointDiscoveryEnabled.Value;
            }
            set { this.endpointDiscoveryEnabled = value; }
        }

        public int EndpointDiscoveryCacheLimit
        {
            get { return this.endpointDiscoveryCacheLimit; }
            set { this.endpointDiscoveryCacheLimit = value; }
        }

        public RequestRetryMode RetryMode
        {
            get
            {
                if (!this.retryMode.HasValue)
                {
                    return FallbackInternalConfigurationFactory.RetryMode ?? DefaultConfiguration.RetryMode;
                }

                return this.retryMode.Value;
            }
            set { this.retryMode = value; }
        }

        public bool FastFailRequests { get; set; } = false;

        public static void ValidateTimeout(TimeSpan? timeout)
        {
            if (!timeout.HasValue)
            {
                throw new ArgumentNullException("timeout");
            }

            if (timeout != InfiniteTimeout && (timeout <= TimeSpan.Zero || timeout > MaxTimeout))
            {
                throw new ArgumentOutOfRangeException("timeout");
            }
        }

#if BCL
        private static void ValidateTcpKeepAliveTimeSpan(TimeSpan? value, string paramName)
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(paramName);
            }

            if (value > MaxTimeout || (int)value.Value.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

#endif
        public static TimeSpan? GetTimeoutValue(TimeSpan? clientTimeout, TimeSpan? requestTimeout)
        {
            return requestTimeout.HasValue ? requestTimeout
                : (clientTimeout.HasValue ? clientTimeout : null);
        }

        public abstract Endpoint DetermineServiceOperationEndpoint(ServiceOperationEndpointParameters parameters);

#if NETSTANDARD
        public bool CacheHttpClient {get; set;} = true;

        private int? _httpClientCacheSize;
        public int HttpClientCacheSize
        {
            get
            {
                if(_httpClientCacheSize.HasValue)
                {
                    return _httpClientCacheSize.Value;
                }

#if NETCOREAPP3_1 || NETCOREAPP3_1_OR_GREATER
                return 1;
#else
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 1 : Environment.ProcessorCount;
#endif
            }
            set => _httpClientCacheSize = value;
        }
#endif

#if NETFRAMEWORK
        private TimeSpan? readWriteTimeout = null;

        public TimeSpan? ReadWriteTimeout
        {
            get { return this.readWriteTimeout; }
            set
            {
                ValidateTimeout(value);
                this.readWriteTimeout = value;
            }
        }
#endif

        public IEndpointProvider EndpointProvider { get; set; }

        public TelemetryProvider TelemetryProvider
        {
            get { return this.telemetryProvider; }
            set { this.telemetryProvider = value; }
        }

        public RequestChecksumCalculation RequestChecksumCalculation
        {
            get
            {
                if (!this.requestChecksumCalculation.HasValue)
                    return FallbackInternalConfigurationFactory.RequestChecksumCalculation ?? RequestChecksumCalculation.WHEN_SUPPORTED;

                return this.requestChecksumCalculation.Value;
            }
            set { requestChecksumCalculation = value; }
        }

        public ResponseChecksumValidation ResponseChecksumValidation
        {
            get
            {
                if (!this.responseChecksumValidation.HasValue)
                    return FallbackInternalConfigurationFactory.ResponseChecksumValidation ?? ResponseChecksumValidation.WHEN_SUPPORTED;

                return this.responseChecksumValidation.Value;
            }
            set { responseChecksumValidation = value; }
        }
        private IWebProxy proxy = null;
        private string proxyHost;
        private int proxyPort = -1;

        private static RegionEndpoint GetDefaultRegionEndpoint()
        {
            return FallbackRegionFactory.GetRegionEndpoint();
        }

        public IWebProxy GetWebProxy()
        {
            return proxy;
        }

        public void SetWebProxy(IWebProxy proxy)
        {
            this.proxy = proxy;
        }

        public string ProxyHost
        {
            get
            {
                if (string.IsNullOrEmpty(this.proxyHost))
                    return AWSConfigs.ProxyConfig.Host;

                return this.proxyHost;
            }
            set
            {
                this.proxyHost = value;
                if (this.ProxyPort>0)
                {
                    this.proxy = new Amazon.Runtime.Internal.Util.WebProxy(ProxyHost, ProxyPort);
                }
            }
        }
        public int ProxyPort
        {
            get
            {
                if (this.proxyPort <= 0)
                    return AWSConfigs.ProxyConfig.Port.GetValueOrDefault();

                return this.proxyPort;
            }
            set
            {
                this.proxyPort = value;
                if (this.ProxyHost!=null)
                {
                    this.proxy = new Amazon.Runtime.Internal.Util.WebProxy(ProxyHost, ProxyPort);
                }
            }
        }
        public int? MaxConnectionsPerServer
        {
            get;
            set;
        }

        public HttpClientFactory HttpClientFactory { get; set; } = AWSConfigs.HttpClientFactory;

        internal static bool CacheHttpClients(IClientConfig clientConfig)
        {
            if (clientConfig.HttpClientFactory == null)
                return clientConfig.CacheHttpClient;
            else
                return clientConfig.HttpClientFactory.UseSDKHttpClientCaching(clientConfig);
        }

        internal static bool DisposeHttpClients(IClientConfig clientConfig)
        {
            if (clientConfig.HttpClientFactory == null)
                return !clientConfig.CacheHttpClient;
            else
                return clientConfig.HttpClientFactory.DisposeHttpClientsAfterUse(clientConfig);
        }

        internal static string CreateConfigUniqueString(IClientConfig clientConfig)
        {
            if (clientConfig.HttpClientFactory != null)
            {
                return clientConfig.HttpClientFactory.GetConfigUniqueString(clientConfig);
            }
            string uniqueString = string.Empty;
            uniqueString = string.Concat("AllowAutoRedirect:", clientConfig.AllowAutoRedirect.ToString(), "CacheSize:", clientConfig.HttpClientCacheSize);

            if (clientConfig.Timeout.HasValue)
                uniqueString = string.Concat(uniqueString, "Timeout:", clientConfig.Timeout.Value.ToString());

            if (clientConfig.MaxConnectionsPerServer.HasValue)
                uniqueString = string.Concat(uniqueString, "MaxConnectionsPerServer:", clientConfig.MaxConnectionsPerServer.Value.ToString());

            return uniqueString;
        }

        internal static bool UseGlobalHttpClientCache(IClientConfig clientConfig)
        {
            if (clientConfig.HttpClientFactory == null)
                return clientConfig.ProxyCredentials == null && clientConfig.GetWebProxy() == null;
            else
                return clientConfig.HttpClientFactory.GetConfigUniqueString(clientConfig) != null;
        }
    }
    */
