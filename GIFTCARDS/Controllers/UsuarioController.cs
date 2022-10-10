using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace GiftCards.Controllers
{
    [Route("api/giftcards/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        protected readonly SampleContext _context;
        public UsuarioController(SampleContext context)
        {
            _context = context;
        }
        [HttpGet("usuario")]
        [ActionName(nameof(GetUsuarioAsync))]
        public IEnumerable GetUsuarioAsync(string usuario, string contrasena)
        {
            var query = (from a in _context.Usuario
                         where a.usuario_nombre.Equals(usuario) && a.contrasena.Equals(contrasena)
                         orderby a.id
                         select new
                         {
                             usuario_id = a.id,
                             a.usuario_nombre,
                             a.correo
                         });
            return query.ToList();
        }


    }
}
