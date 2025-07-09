namespace MassTransit.ScalewaySnsTransport
{
    using System.Net;
    using Amazon.Runtime;


    public static class ScalewayResponseExtensions
    {
        public static void EnsureSuccessfulResponse(this AmazonWebServiceResponse response)
        {
            var statusCode = response.HttpStatusCode;

            if (statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.MultipleChoices)
                return;

            var requestId = response.ResponseMetadata?.RequestId ?? "[Missing RequestId]";

            throw new ScalewaySnsTransportException(
                $"Received unsuccessful response ({statusCode}) from Scaleway endpoint");
        }
    }
}
