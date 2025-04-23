using TransitRealtime;

namespace MetrobusWorkerService.Services;
public interface IFunctionSenderService
{
    Task SendToFunctionAsync(FeedMessage feed);
}
