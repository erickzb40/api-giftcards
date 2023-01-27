using GiftCards.entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftCards;
using Asistencia_apirest.services;
using SelectPdf;
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

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromHeader] string token,string id)
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
                var query = await context.carddets.Where(a=>a.id_cab.Equals(id)&&a.estado.Equals(1)).ToListAsync();
                if (query==null) {
                    return Problem("No se encontro giftcards activos");
                }
                var a= _convert.convertirHtmlPdf(query);
                return Ok(a);
            }

           
         }

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Erick", "zegarrabarrera.erick@gmail.com"));
            //message.To.Add(new MailboxAddress("andre", "erickzb40@gmail.com"));
            //message.Subject = "prueba";

            //message.Body = new TextPart("plain")
            //{
            //    Text = "mensaje de prueba"
            //};

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.gmail.com", 587);
            //    client.Authenticate("zegarrabarrera.erick@gmail.com", "duajxgsczrrjbngh");
            //    client.Send(message);
            //    client.Disconnect(true);
            //}
            
     }
}

