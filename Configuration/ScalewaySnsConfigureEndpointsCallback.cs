namespace MassTransit;

public delegate void ScalewaySnsConfigureEndpointsCallback(IRegistrationContext context, string queueName, IScalewaySnsReceiveEndpointConfigurator configurator);
