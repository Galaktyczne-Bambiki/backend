using BambikiBackend.Api.Database;
using BambikiBackend.Api.Database.Entities;
using BambikiBackend.Api.Models.FireReports;
using BambikiBackend.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace BambikiBackend.Api.Services;

public class ReportsService
{
    private readonly BambikiDatabaseContext _databaseContext;

    public ReportsService(BambikiDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task AddReport(FireReportRequestModel model, CancellationToken cancellationToken)
    {
        var entity = new FireReportsEntity()
        {
            Description = model.Details.Description
        };

        var image = await Image.LoadAsync(model.File.OpenReadStream(), cancellationToken);

        using var memoryStream = new MemoryStream();
        await model.File.CopyToAsync(memoryStream, cancellationToken);
        entity.Image = memoryStream.ToArray();

        var imageCords = await ExifExtensions.ExtractCoordinates(image);
        entity.Latitude = model.Details.Latitude ?? imageCords.Lat.Value;
        entity.Longitude = model.Details.Longitude ?? imageCords.Lng.Value;


        await _databaseContext.FireReports.AddAsync(entity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<FireReportsEntity>> GetAll(CancellationToken cancellationToken)
    {
        return await _databaseContext.FireReports.ToListAsync(cancellationToken);
    }
}