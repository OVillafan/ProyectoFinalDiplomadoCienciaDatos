using TransitRealtime;

public interface IMetrobusDataService
{
    Task<FeedMessage?> GetFeedMessageAsync();
}