namespace MassTransit
{
    public class ScalewaySnsTransportOptions
    {
        public string Region { get; set; }
        public string Scope { get; set; }
        public string SqsAccessKey { get; set; }
        public string SnsAccessKey { get; set; }
        public string SqsSecretKey { get; set; }
        public string SnsSecretKey { get; set; }
    }
}
