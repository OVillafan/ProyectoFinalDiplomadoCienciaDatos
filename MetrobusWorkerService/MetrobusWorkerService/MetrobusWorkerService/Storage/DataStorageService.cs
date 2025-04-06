public class DataStorageService
{
    public Task SaveAsync(List<MetrobusData> data)
    {
        foreach (var item in data)
        {
            Console.WriteLine($"{item.VehicleId}: {item.Latitude}, {item.Longitude} @ {item.Timestamp}");
        }

        return Task.CompletedTask;
    }
}
