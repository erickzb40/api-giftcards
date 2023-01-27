
using Asistencia_apirest.services;
using GiftCards.entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class giftcarddetsController : ControllerBase
    {
        private readonly SampleContext _context;
        public cifrado _cifrado;
        public util _util;

        public giftcarddetsController(SampleContext context,cifrado cifrado_,util util_)
        {
            _context = context;
            _cifrado = cifrado_;
            _util = util_;
        }

        // GET: api/giftcarddets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<giftcarddet>>> Getcarddets([FromHeader] string token,string desde,string hasta)
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
            if (empresa.cadenaconexion == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                }
                if (context.carddets == null)
                {
                    return NotFound();
                }
                return await   context.carddets.Where(a=>a.fecha_creado>=DateTime.Parse(desde)&&a.fecha_creado<= DateTime.Parse(hasta).AddDays(1).AddSeconds(-1)).ToListAsync();
            }
        }

        // GET: api/giftcarddets/5
        [HttpGet("{documento}")]
        public async Task<ActionResult<giftcarddet>> Getgiftcarddet(string documento, [FromHeader] string token)
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
            if (empresa.cadenaconexion == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                }
                if (context.carddets == null)
                {
                    return NotFound();
                }
                var giftcarddet = await (from a in context.carddets where a.documento_ref.Equals(documento) select a).ToListAsync();

                if (giftcarddet.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(giftcarddet);
            }
        }

        [HttpGet("serie/{serie}")]
        public async Task<ActionResult<giftcarddet>> GetSerie(string serie, [FromHeader] string token)
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
            if (empresa.cadenaconexion == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                }
                var usuario_locales = await context.Usuario_local.Where(res => res.usuarioid.Equals(usuario.usuarioid)).ToListAsync();
                if (usuario_locales == null)
                {
                    return Problem("No hay locales asignados");
                }
                if (context.carddets == null)
                {
                    return Content("[]");
                }
                //int[] locales = _util.convertirArray(usuario_locales);
                var giftcarddet =await (from a in context.carddets
                                  join b in context.local on a.local equals b.id
                                  join c in context.estado on a.estado equals c.id
                                  where a.serie.Equals(serie)
                                  select new
                                  {
                                      a.id,
                                      a.serie,
                                      a.documento_ref,
                                      a.estado,
                                      a.fecha_vencimiento,
                                      local_descripcion = b.descripcion,
                                      a.monto,
                                      estado_descripcion = c.descripcion,
                                      a.local,
                                      a.id_cab
                                  }).ToListAsync();

                if (giftcarddet.Count() == 0)
                {
                    return Content("[]");
                }

                return Ok(giftcarddet);
            }
        }

        // PUT: api/demo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> Putgiftcarddet([FromHeader] string token,giftcarddet giftcarddet)
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
            if (empresa.cadenaconexion == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                } 
                var giftcarddetresult = await context.carddets.FirstOrDefaultAsync(res => res.id == giftcarddet.id);
                if (giftcarddetresult == null)
                {
                    return Problem("El GiftCard no existe");
                }
                var activo = from a in context.cardcabs
                             join dets in context.carddets on a.id equals dets.id_cab
                             where dets.estado.Equals(1) && dets.serie.Equals(giftcarddet.serie)
                             select a;
                if (activo.Count() == 0)
                {
                    return Problem("El GiftCard no se encuentra activo");
                }

                giftcarddetresult.fecha_canje = DateTime.Now;
                giftcarddetresult.usuario = usuario.usuarioid;
                giftcarddetresult.estado = 2;
                //_context.Entry(giftcarddet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
