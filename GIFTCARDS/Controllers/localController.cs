
using Microsoft.AspNetCore.Mvc;

namespace GiftCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class localController : ControllerBase
    {
        private readonly SampleContext _context;

        public localController(SampleContext context)
        {
            _context = context;
        }
    }
}
