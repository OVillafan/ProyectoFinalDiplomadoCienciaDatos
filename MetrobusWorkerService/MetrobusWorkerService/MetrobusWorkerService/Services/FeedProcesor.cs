using Google.Protobuf;
using System.Collections.Generic;
using TransitRealtime;

public class FeedProcessor
{
    public List<MetrobusData> Process(byte[] protobufData)
    {
        var feed = FeedMessage.Parser.ParseFrom(protobufData);
        var result = new List<MetrobusData>();

        foreach (var entity in feed.Entity)
        {
            if (entity?.Vehicle != null)
            {
                var vehicle = entity.Vehicle;
                var position = vehicle.Position;

                result.Add(new MetrobusData
                {
                    VehicleId = vehicle.Vehicle?.Id ?? "N/A",
                    Latitude = position?.Latitude ?? 0,
                    Longitude = position?.Longitude ?? 0,
                    Timestamp = (long)vehicle.Timestamp
                });
            }
        }

        return result;
    }
}
