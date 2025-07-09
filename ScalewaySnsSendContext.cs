namespace MassTransit
{
    public interface ScalewaySnsSendContext<out T> :
        ScalewaySnsSendContext,
        SendContext<T>
        where T : class
    {
    }


    public interface ScalewaySnsSendContext :
        SendContext
    {
        string GroupId { set; }
        string DeduplicationId { set; }
        int? DelaySeconds { set; }
    }
}
