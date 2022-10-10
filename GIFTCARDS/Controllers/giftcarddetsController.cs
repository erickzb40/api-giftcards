
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

        public giftcarddetsController(SampleContext context)
        {
            _context = context;
        }

        // GET: api/giftcarddets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<giftcarddet>>> Getcarddets()
        {
            if (_context.carddets == null)
            {
                return NotFound();
            }
            return await _context.carddets.ToListAsync();
        }

        // GET: api/giftcarddets/5
        [HttpGet("{documento}")]
        public async Task<ActionResult<giftcarddet>> Getgiftcarddet(string documento)
        {
            if (_context.carddets == null)
            {
                return NotFound();
            }
            var giftcarddet = await (from a in _context.carddets where a.documento_ref.Equals(documento) select a).ToListAsync();

            if (giftcarddet.Count() == 0)
            {
                return NotFound();
            }

            return Ok(giftcarddet);
        }

        [HttpGet("serie/{serie}")]
        public ActionResult<giftcarddet> GetSerie(string serie)
        {
            if (_context.carddets == null)
            {
                return Content("[]");
            }
            var giftcarddet = from a in _context.carddets
                              join b in _context.local on a.local equals b.id
                              join c in _context.estado on a.estado equals c.id
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
                              };

            if (giftcarddet.Count() == 0)
            {
                return Content("[]");
            }

            return Ok(giftcarddet);
        }

        // PUT: api/demo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> Putgiftcarddet(giftcarddet giftcarddet)
        {
            var usuario = from z in _context.Usuario where z.usuario_nombre.Equals(giftcarddet.nombre_usuario) select z;
            if (usuario.Count() == 0)
            {
                return Problem("No tiene los permisos para realizar esta acción");
            }
            var giftcarddetresult = await _context.carddets.FirstOrDefaultAsync(res => res.id == giftcarddet.id);
            if (giftcarddetresult == null)
            {
                return Problem("El GiftCard no existe");
            }
            var activo = from a in _context.cardcabs
                         join dets in _context.carddets on a.id equals dets.id_cab
                         where dets.estado.Equals(1) && dets.serie.Equals(giftcarddet.serie)
                         select a;
            if (activo.Count() == 0)
            {
                return Problem("El GiftCard no se encuentra activo");
            }

            giftcarddetresult.fecha_canje = DateTime.Now;
            giftcarddetresult.usuario = usuario.FirstOrDefault()!.id;
            giftcarddetresult.estado = 2;
            //_context.Entry(giftcarddet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool giftcarddetExists(string serie)
        {
            return (_context.carddets?.Any(e => e.serie == serie)).GetValueOrDefault();
        }
    }
}
