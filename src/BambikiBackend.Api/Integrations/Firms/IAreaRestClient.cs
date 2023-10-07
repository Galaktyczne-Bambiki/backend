using System.Globalization;
using BambikiBackend.Api.Integrations.Firms.Models;
using Refit;

namespace BambikiBackend.Api.Integrations.Firms;

public interface IAreaRestClient
{
    [Get("/api/area/csv/[[API_KEY]]/VIIRS_SNPP_NRT/{west},{south},{east},{north}/1/{date}")]
    Task<string> GetCsvAreaDataAsync(string date, string west, string south, string east, string north, CancellationToken cancellationToken);

    public async Task<ICollection<AreaDataModel>> GetAreaData(DateOnly date, AreaCords cords, CancellationToken cancellationToken)
    {
        var data = await GetCsvAreaDataAsync(date.ToString("yyyy-MM-dd"), cords.West.ToString(CultureInfo.InvariantCulture), cords.South.ToString(CultureInfo.InvariantCulture), cords.East.ToString(CultureInfo.InvariantCulture), cords.North.ToString(CultureInfo.InvariantCulture), cancellationToken);

        return data
            .Split('\n')
            .Skip(1)
            .Select(line => new AreaDataModel(line))
            .ToList();
    }
}