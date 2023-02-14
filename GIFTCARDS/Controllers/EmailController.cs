using GiftCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asistencia_apirest.services;
using GIFTCARDS.metodos;

namespace GIFTCARDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly SampleContext _context;
        public cifrado _cifrado;
        public Conversion _convert;

        public EmailController(SampleContext context, cifrado cifrado_,Conversion convert_)
        {
            _context = context;
            _cifrado = cifrado_;
            _convert = convert_;
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail([FromHeader] string token, int id)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return Problem("El token no es valido!");
            }
            var empresa = await _context.Empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] && x.app.Equals("GIFTCARDS"));
            if (empresa == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var cabecera = await context.cardcabs.Where(a => a.id.Equals(id)).FirstOrDefaultAsync();
                if (cabecera == null)
                {
                    return Problem("No se encontro ningun registro");
                }
                if (cabecera.correo == null || cabecera.correo == "")
                {
                    return Problem("No se encontro ningun correo registrado");
                }
                var detalle = await context.carddets.Where(a => a.id_cab.Equals(id) && a.estado.Equals(1)).ToListAsync();
                if (detalle == null)
                {
                    return Problem("No se encontro giftcards activos");
                }
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                }
                var usuario_local = await context.Usuario_local.Where(a => a.usuarioid.Equals(usuario.usuarioid)).FirstOrDefaultAsync();
                var local = await context.local.Where(a => a.id.Equals(usuario_local.localid)).FirstOrDefaultAsync();
                if (local==null) {
                    return Problem("No esta configurado un local para este usuario!");
                }
                _convert.convertirGiftcardEmail(detalle, cabecera, local);
                return Ok(detalle.Count);
            }
        }
     }
}

