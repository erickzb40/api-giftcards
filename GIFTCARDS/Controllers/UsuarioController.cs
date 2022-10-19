using Asistencia_apirest.services;
using GiftCards.entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftCards.Controllers
{
    [Route("api/giftcards/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        protected readonly SampleContext _context;
        private cifrado _cifrado;
        public UsuarioController(SampleContext context, cifrado cifrado_)
        {
            _cifrado = cifrado_;
            _context = context;
        }
        [HttpPost("usuario")]
        public async Task<IActionResult> GetUsuariosAsync(Usuario usuario)
        {
            var query = await _context.Empresa.FirstOrDefaultAsync(res => res.descripcion.Equals(usuario.empresa) && res.app.Equals("GIFTCARDS"));
            if (query == null)
            {
                return Problem("No se encontro la empresa");
            }
            if (query.cadenaconexion == null)
            {
                return Problem("No se encontro la empresa");
            }
            using (var context = new SampleContext(query.cadenaconexion))
            {
                var result = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(usuario.nombreusuario) && res.contrasena.Equals(usuario.contrasena));
                if (result == null)
                {
                    return Problem("No se encontro ningun usuario");
                }
                var cifrado = _cifrado.EncryptStringAES(usuario.empresa + " " + usuario.nombreusuario + " " + usuario.contrasena);
                return Ok("{\"token\":\"" + cifrado + "\"}");
            }
        }


    }
}
