
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



        // GET: api/local/5
        //[HttpGet]
        //public ActionResult<local> Getlocal(string a,string p)
        //{
        //    var usr = (from e in _context.Usuario.Where(res=>res.usuario_nombre!.Equals(a)&&res.contrasena!.Equals(p)) 
        //               select new {e.local});
        //    if (usr.Count()==0)
        //    {
        //        return Problem("No tiene los permisos para relizar esta acción");
        //    }
        //    if (_context.local == null)
        //    {
        //          return NotFound();
        //    }

        //    var local = from b in _context.local select b;

        //    if (local == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(local.ToList());
        //}

        //private bool localExists(int id)
        //{
        //    return (_context.local?.Any(e => e.id == id)).GetValueOrDefault();
        //}
    }
}
