namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;


    public interface IScalewaySnsTopologyConfiguration :
        ITopologyConfiguration
    {
        new IScalewaySnsPublishTopologyConfigurator Publish { get; }

        new IScalewaySnsSendTopologyConfigurator Send { get; }

        new IScalewaySnsConsumeTopologyConfigurator Consume { get; }
    }
}
