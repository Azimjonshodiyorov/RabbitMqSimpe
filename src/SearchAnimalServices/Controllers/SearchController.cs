using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchAnimalServices.Helpers;
using SearchAnimalServices.Models;

namespace SearchAnimalServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Animal>>> SearchAnimals([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Animal, Animal>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        // Sort by parameters
        query = searchParams.OrderBy switch
        {
            "age" => query.Sort(x => x.Ascending(y => y.Age)),
            "weight" => query.Sort(x => x.Ascending(y => y.Weight)),
            _ => query.Sort(x => x.Ascending(y => y.CreatedAt)),
        };

        // Filter by parameters
        query = searchParams.FilterBy switch
        {
            "found" => query.Match(x => x.Status == "Found"),
            "pending" => query.Match(x => x.Status == "Pending"),
            "available" => query.Match(x => x.Status == "Available"),
            "missing" => query.Match(x => x.Status == "Missing"),
            _ => query.Sort(x => x.Ascending(y => y.CreatedAt)),
        };

        if (!string.IsNullOrEmpty(searchParams.Type))
        {
            query.Match(x => x.Type == searchParams.Type);
        }

        if (!string.IsNullOrEmpty(searchParams.Sex))
        {
            query.Match(x => x.Sex == searchParams.Sex);
        }


        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}