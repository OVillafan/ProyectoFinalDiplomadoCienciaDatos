public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMetrobusDataService _metrobusService;

    public Worker(ILogger<Worker> logger, IMetrobusDataService metrobusService)
    {
        _logger = logger;
        _metrobusService = metrobusService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var feed = await _metrobusService.GetFeedMessageAsync();

            if (feed != null)
            {
                var i = 0;
                foreach (var entity in feed.Entity)
                {
                    if (entity?.Vehicle != null)
                    {
                        var pos = entity.Vehicle.Position;
                        _logger.LogInformation($"Vehículo CS {entity.Vehicle.CurrentStatus} CongLevel ({entity.Vehicle?.CongestionLevel}  ) {i++}");
                       
                    }
                }
            }

            await Task.Delay(30000, stoppingToken);
        }
    }
}
