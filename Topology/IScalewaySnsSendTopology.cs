namespace MassTransit
{
    using ScalewaySnsTransport;


    public interface IScalewaySnsSendTopology :
        ISendTopology
    {
        new IScalewaySnsMessageSendTopologyConfigurator<T> GetMessageTopology<T>()
            where T : class;

        SendSettings GetSendSettings(ScalewaySnsEndpointAddress address);

        /// <summary>
        /// Return the error settings for the queue
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        ErrorSettings GetErrorSettings(ReceiveSettings settings);

        /// <summary>
        /// Return the dead letter settings for the queue
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        DeadLetterSettings GetDeadLetterSettings(ReceiveSettings settings);
    }
}
