//using MetrobusWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient();
//builder.Services.AddScoped<DataFetcher>();
builder.Services.AddTransient<IMetrobusDataService,MetrobusDataService>();
//builder.Services.AddScoped<FeedProcessor>();
//builder.Services.AddScoped<DataStorageService>();

var host = builder.Build();
host.Run();
