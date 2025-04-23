using MetrobusWorkerService.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMetrobusDataService _metrobusService;
    private readonly IFunctionSenderService _functionSender;

    public Worker(ILogger<Worker> logger, IMetrobusDataService metrobusService, IFunctionSenderService functionSender)
    {
        _logger = logger;
        _metrobusService = metrobusService;
        _functionSender = functionSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var feed = await _metrobusService.GetFeedMessageAsync();
            if (feed != null)
            {
                await _functionSender.SendToFunctionAsync(feed);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}
