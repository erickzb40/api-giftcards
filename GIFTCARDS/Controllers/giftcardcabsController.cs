
using GiftCards.entity;
using GiftCards.metodos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class giftcardcabsController : ControllerBase
    {
        private readonly SampleContext _context;
        public GenerarCodigoCupon _codigoCupon;
        public SecurityManager _Aes;

        public giftcardcabsController(SampleContext context, GenerarCodigoCupon cupon, SecurityManager sm)
        {
            _context = context;
            _codigoCupon = cupon;
            _Aes = sm;
        }

        // GET: api/cardcabs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<giftcardcab>>> Getcardcabs()
        {
            if (_context.cardcabs == null)
            {
                return NotFound();
            }
            return await _context.cardcabs.ToListAsync();
        }

        // GET: api/cardcabs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<giftcardcab>> Getgiftcardcab(int id)
        //{
        //  if (_context.cardcabs == null)
        //  {
        //      return NotFound();
        //  }
        //    var giftcardcab = await _context.cardcabs.FindAsync(id);

        //    if (giftcardcab == null)
        //    {
        //        return NotFound();
        //    }

        //    return giftcardcab;
        //}

        [HttpPut]
        public async Task<ActionResult> Putgiftcardcab(giftcardcab giftcardcab)
        {
            if (giftcardcab == null) { return Problem(); }
            if (giftcardcab == null) { return Problem("No se ha enviado el id"); }
            if (giftcardcab == null) { return Problem("No se ha enviado el estado"); }
            if (!giftcardcabExists(giftcardcab.id)) { return Problem("No existe el registro"); }
            var cab = await _context.cardcabs.FirstOrDefaultAsync(res => res.id == giftcardcab.id);
            if (cab != null)
            {
                var det = await _context.carddets.Where(res => res.id_cab == giftcardcab.id).ToListAsync();
                foreach (var d in det)
                {
                    if (d.estado != 2)
                    {
                        d.estado = giftcardcab.estado;
                    }
                }
                cab.estado = giftcardcab.estado;
                _context.SaveChanges();

            }
            return Ok(cab);
        }

        // POST: api/cardcabs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<giftcardcab>> Postgiftcardcab(giftcardcab _giftcardcab, string us)
        {
            var result = await _context.Usuario.Where(res => res.usuario_nombre.Equals(us)).ToListAsync();
            var documento = from u in _context.cardcabs where u.documento!.Equals(_giftcardcab.documento) select u;
            if (result.Count() == 0)
            {
                return Problem("No tiene los permisos para relizar esta acción");
            }
            if (documento.Count() > 0)
            {
                return Problem("El documento ingresado ya ha sido registrado!");
            }
            if (_context.cardcabs == null || _giftcardcab.cantidad < 1)
            {
                return Problem("Se ingreso un valor nulo o la cantidad es menor a 1");
            }
            if (_giftcardcab.montoTexto == null)
            {
                return Problem("El campo mostrar es requerido!");
            }
            _giftcardcab.estado = 5;
            _giftcardcab.usuario = result.First().id;
            _giftcardcab.local = result.First().local;
            _context.cardcabs.Add(_giftcardcab);
            await _context.SaveChangesAsync();
            for (int i = 0; i < _giftcardcab.cantidad; i++)
            {
                var dpc = _giftcardcab.id.ToString() + _giftcardcab.cantidad + _giftcardcab.importe + i;
                giftcarddet gdet = new giftcarddet
                {
                    serie = "LH" + _codigoCupon.couponCode(int.Parse(dpc)),
                    monto = _giftcardcab.importe,
                    id_cab = _giftcardcab.id,
                    documento_ref = _giftcardcab.documento!,
                    local = result.First().local,
                    fecha_vencimiento = _giftcardcab.fecha_vencimiento,
                    estado = 5,
                    mostrar = _giftcardcab.montoTexto,
                    descripcion = _giftcardcab.descripcion


                };
                _context.carddets.Add(gdet);

            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/cardcabs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Deletegiftcardcab(int id)
        //{
        //    if (_context.cardcabs == null)
        //    {
        //        return NotFound();
        //    }
        //    var giftcardcab = await _context.cardcabs.FindAsync(id);
        //    if (giftcardcab == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.cardcabs.Remove(giftcardcab);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool giftcardcabExists(int id)
        {
            return (_context.cardcabs?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
