namespace eAuction.BaseLibrary.Domain
{

    public interface IAggregateRoot
    {
        string Id { get; }
        object GetId();
    }

}
