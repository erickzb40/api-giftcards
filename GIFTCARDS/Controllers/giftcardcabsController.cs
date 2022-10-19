
using Asistencia_apirest.services;
using GiftCards.entity;
using GiftCards.metodos;
using GIFTCARDS.entity;
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
        public cifrado _cifrado;
        public util _util;

        public giftcardcabsController(util util_, SampleContext context, GenerarCodigoCupon cupon, SecurityManager sm, cifrado cifrado_)
        {
            _context = context;
            _codigoCupon = cupon;
            _Aes = sm;
            _cifrado = cifrado_;
            _util = util_;
        }

        // GET: api/cardcabs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<giftcardcab>>> Getcardcabs(string token)
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
                if (context.cardcabs == null)
                {
                    return NotFound();
                }
                return await context.cardcabs.ToListAsync();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Putgiftcardcab(giftcardcab giftcardcab)
        {

            var vtoken = _cifrado.validarToken(giftcardcab.token);
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
                int[] locales = _util.convertirArray(usuario_locales);
                if (giftcardcab == null) { return Problem("Ingrese todos los campos"); }
                var cab = await context.cardcabs.FirstOrDefaultAsync(c => c.id == giftcardcab.id);
                if (cab==null) { return Problem("No existe el registro"); }
                if (cab != null)
                {
                    var det = await context.carddets.Where(res => res.id_cab == giftcardcab.id).ToListAsync();
                    foreach (var d in det)
                    {
                        if (d.estado != 2)
                        {
                            d.estado = giftcardcab.estado;
                        }
                    }
                    User_aprob_log user_Aprob_Log = new User_aprob_log 
                    {
                        usuario = usuario.usuarioid,
                        id_cab_giftcard = giftcardcab.id,
                        estado = giftcardcab.estado,
                     };
                    context.user_aprob_log.Add(user_Aprob_Log);
                    cab.estado = giftcardcab.estado;
                    context.SaveChanges();

                }
                return Ok(cab);
            }
        }

        [HttpPost]
        public async Task<ActionResult<giftcardcab>> Postgiftcardcab(giftcardcab _giftcardcab)
        {
            var vtoken = _cifrado.validarToken(_giftcardcab.token);
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
                var usuario_local = await context.Usuario_local.FirstOrDefaultAsync(res=>res.usuarioid.Equals(usuario.usuarioid));
                var documento = from u in context.cardcabs where u.documento!.Equals(_giftcardcab.documento) select u;
                if (usuario == null)
                {
                    return Problem("No tiene los permisos para relizar esta acción");
                }
                if (documento.Count() > 0)
                {
                    return Problem("El documento ingresado ya ha sido registrado!");
                }
                if (context.cardcabs == null || _giftcardcab.cantidad < 1)
                {
                    return Problem("Se ingreso un valor nulo o la cantidad es menor a 1");
                }
                if (_giftcardcab.montoTexto == null)
                {
                    return Problem("El campo mostrar es requerido!");
                }
                if (usuario_local==null)
                {
                    return Problem("El usuario no tiene un local asignado!");
                }
                _giftcardcab.estado = 5;
                _giftcardcab.usuario = usuario.usuarioid;
                _giftcardcab.local = usuario_local.localid;
                context.cardcabs.Add(_giftcardcab);
                await context.SaveChangesAsync();
                for (int i = 0; i < _giftcardcab.cantidad; i++)
                {
                    var dpc = _giftcardcab.id.ToString() + _giftcardcab.cantidad + _giftcardcab.importe + i;
                    giftcarddet gdet = new giftcarddet
                    {
                        serie = "LH" + _codigoCupon.couponCode(int.Parse(dpc)),
                        monto = _giftcardcab.importe,
                        id_cab = _giftcardcab.id,
                        documento_ref = _giftcardcab.documento!,
                        fecha_vencimiento = _giftcardcab.fecha_vencimiento,
                        estado = 5,
                        mostrar = _giftcardcab.montoTexto,
                        descripcion = _giftcardcab.descripcion,
                        local = usuario_local.localid
                };
                    context.carddets.Add(gdet);

                }
                await context.SaveChangesAsync();
                return Ok();
            }

        }
    }
}
