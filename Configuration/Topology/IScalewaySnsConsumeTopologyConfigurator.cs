namespace MassTransit
{
    public interface IScalewaySnsConsumeTopologyConfigurator :
        IConsumeTopologyConfigurator,
        IScalewaySnsConsumeTopology
    {
        new IScalewaySnsMessageConsumeTopologyConfigurator<T> GetMessageTopology<T>()
            where T : class;

        void AddSpecification(IScalewaySnsConsumeTopologySpecification specification);
    }
}
