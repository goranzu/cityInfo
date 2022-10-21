using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities/{cityId:int}/pointsofinterest")]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city is null)
            return NotFound();

        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{pointId:int}")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointId)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city is null)
            return NotFound();

        var point = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointId);
        if (point is null)
            return NotFound();

        return Ok(point);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto point)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city is null)
            return NotFound();

        var maxPointIfInterest = CitiesDataStore.Current.Cities
            .SelectMany(c => c.PointsOfInterest)
            .Max(p => p.Id);

        var pointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointIfInterest,
            Name = point.Name,
            Description = point.Description
        };

        city.PointsOfInterest.Add(pointOfInterest);

        return CreatedAtAction(
            nameof(GetPointOfInterest),
            new { cityId, pointId = pointOfInterest.Id },
            pointOfInterest);
    }

    [HttpPut("{pointId}")]
    public ActionResult UpdatePointOfInterest(int cityId, int pointId, PointOfInterestForUpdateDto updateDto)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city is null)
            return NotFound();

        var point = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointId);
        if (point is null)
            return NotFound();

        point.Name = updateDto.Name;
        point.Description = updateDto.Description;
        return NoContent();
    }
}