using BambikiBackend.Api.Integrations.Firms.Models;
using Refit;

namespace BambikiBackend.Api.Integrations.Firms;

public interface IAreaRestClient
{
    [Get("/api/area/csv/[[API_KEY]]/VIIRS_SNPP_NRT/{west},{south},{east},{north}/1/{date}")]
    Task<string> GetCsvAreaDataAsync(string date, long west, long south, long east, long north, CancellationToken cancellationToken);

    public async Task<ICollection<AreaDataModel>> GetAreaData(DateOnly date, AreaCords cords, CancellationToken cancellationToken)
    {
        var data = await GetCsvAreaDataAsync(date.ToString("yyyy-MM-dd"), cords.West, cords.South, cords.East, cords.North, cancellationToken);

        return data
            .Split('\n')
            .Skip(1)
            .Select(line => new AreaDataModel(line))
            .ToList();
    }
}