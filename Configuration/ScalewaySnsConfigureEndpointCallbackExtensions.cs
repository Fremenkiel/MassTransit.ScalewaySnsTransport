namespace MassTransit;

using System;


public static class ScalewaySnsConfigureEndpointCallbackExtensions
{
    /// <summary>
    /// Add an ScalewaySns specific configure callback to the endpoint.
    /// </summary>
    /// <param name="configurator"></param>
    /// <param name="callback"></param>
    public static void AddScalewaySnsConfigureEndpointCallback(this IEndpointRegistrationConfigurator configurator,
        Action<IRegistrationContext, IScalewaySnsReceiveEndpointConfigurator> callback)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        configurator.AddConfigureEndpointCallback((context, cfg) =>
        {
            if (cfg is IScalewaySnsReceiveEndpointConfigurator sb)
                callback(context, sb);
        });
    }

    /// <summary>
    /// Add an ScalewaySns specific configure callback for configured endpoints
    /// </summary>
    /// <param name="configurator"></param>
    /// <param name="callback"></param>
    public static void AddScalewaySnsConfigureEndpointsCallback(this IBusRegistrationConfigurator configurator, ScalewaySnsConfigureEndpointsCallback callback)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        configurator.AddConfigureEndpointsCallback((context, name, cfg) =>
        {
            if (cfg is IScalewaySnsReceiveEndpointConfigurator sb)
                callback(context, name, sb);
        });
    }
}
