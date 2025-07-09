namespace MassTransit
{
    using System;


    public interface IScalewaySnsSendTopologyConfigurator :
        ISendTopologyConfigurator,
        IScalewaySnsSendTopology
    {
        Action<IScalewaySnsQueueConfigurator> ConfigureErrorSettings { set; }
        Action<IScalewaySnsQueueConfigurator> ConfigureDeadLetterSettings { set; }
    }
}
