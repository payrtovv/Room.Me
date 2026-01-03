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
            var features = await _Context.Feature.Select(r => new
            {
                r.Name,
                r.Id
            }).ToListAsync();

            return Ok(new
            {
                Features = features
            });
        }




    }
}
