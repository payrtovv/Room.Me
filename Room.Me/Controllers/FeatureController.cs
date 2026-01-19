using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;

namespace Room.Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : ControllerBase
    {
        private readonly RoomMeDbContext _Context;


        public FeatureController(RoomMeDbContext context)
        {
            _Context = context;
        }
        [HttpGet("GetAllFeatures")]
        public async Task<ActionResult> GetDefaultFeatures()
        {
            var features = await _Context.Feature.ToListAsync();

            var groupedFeatures = features
                .GroupBy(f => f.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(f => new
                    {
                        f.Id,
                        f.Name,
                        f.Key,
                    }).ToList()
                );

            return Ok(new
            {
                Features = groupedFeatures
            });
        }




    }
}
