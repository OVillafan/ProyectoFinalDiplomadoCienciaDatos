//using MetrobusWorkerService;

using MetrobusWorkerService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IMetrobusDataService,MetrobusDataService>();
builder.Services.AddTransient<IFunctionSenderService, FunctionSenderService>();
//string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

var host = builder.Build();
host.Run();
