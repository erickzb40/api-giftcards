using GiftCards.entity;
using MailKit.Net.Smtp;
using MimeKit;
using SelectPdf;
using System.IO.Compression;

namespace GIFTCARDS.metodos
{
    public class Conversion
    {
        public void convertirGiftcardEmail(List<giftcarddet> det,giftcardcab cab,Local local) {
 
            try
            {
                // Código que puede generar una excepción
                int ancho = 720;
                int alto = 480;
                byte[] rar = this.ConvertirYComprimirImagenes(det, ancho, alto);
                this.enviarCorreo(cab, rar,local);
            }
            catch (Exception ex)
            {
                // Crea una ruta para la carpeta de logs si no existe
                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }

                // Escribir el mensaje de error en un archivo de texto en la carpeta de logs
                using (StreamWriter writer = new StreamWriter("logs/error.log", true))
                {
                    writer.WriteLine("Fecha: " + DateTime.Now.ToString());
                    writer.WriteLine("Mensaje de error: " + ex.Message);
                    writer.WriteLine("Stack Trace: " + ex.StackTrace);
                    writer.WriteLine("");
                }
            }
        }
        public byte[] ConvertirYComprimirImagenes(List<giftcarddet> det, int ancho, int alto)
        {
            List<byte[]> imagenes = new List<byte[]>();

            foreach (giftcarddet item in det)
            {
                string htmlContent = System.IO.File.ReadAllText("img/index.txt");
                string[] fecha = item.fecha_vencimiento.ToString().Split(' ');
                htmlContent = htmlContent.Replace("VMONTO", item.monto.ToString());
                htmlContent = htmlContent.Replace("VSERIE", item.serie.ToString());
                htmlContent = htmlContent.Replace("VFECHA", fecha[0]);
                HtmlToImage hti = new HtmlToImage(ancho, alto);
                var img = hti.ConvertHtmlString(htmlContent);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    memoryStream.Position = 0;
                    imagenes.Add(memoryStream.ToArray());
                }
            }

            using (MemoryStream memoryStreamRAR = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStreamRAR, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < det.Count; i++)
                    {
                        ZipArchiveEntry entry = zipArchive.CreateEntry(det[i].serie + ".jpg");
                        using (Stream entryStream = entry.Open())
                        {
                            entryStream.Write(imagenes[i], 0, imagenes[i].Length);
                        }
                    }
                }
                return memoryStreamRAR.ToArray();
            }
        }

        public void enviarCorreo(giftcardcab cab, byte[] listaBytesRar,Local local)
        {
            
            try {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("JK", "desarrollo@jkconsultores.com"));
                message.To.Add(new MailboxAddress(cab.nombre, cab.correo));
                // Establecer el asunto del mensaje
                message.Subject = "Giftcards";

                // Crear una nueva instancia de Multipart
                var multipart = new Multipart();

                // Crear una nueva instancia de TextPart y establecer el cuerpo del mensaje
                var textpart = new TextPart("plain")
                {
                    Text = "En este mensaje se le adjunta los codigos de los giftcards activos que puede usar para realizar el canje en los lugares permitidos"
                };
                // Agregar el TextPart al Multipart
                multipart.Add(textpart);

                var attachment = new MimePart("application", "zip")
                {
                    Content = new MimeContent(new MemoryStream(listaBytesRar)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "giftcards.zip"
                };
                // Agregar el MimePart al Multipart
                multipart.Add(attachment);
                // Establecer el cuerpo del mensaje como el Multipart
                message.Body = multipart;
                using (var client = new SmtpClient())
                {
                    client.Connect("mail.jkconsultores.com", local.puerto);
                    client.Authenticate(local.correo, local.contrasena);
                    client.Send(message);
                    client.Disconnect(true);
                }
                Console.WriteLine(local.correo+"-"+ local.contrasena + "-" + local.puerto);
            }
            catch (Exception ex)
            {
                // Crea una ruta para la carpeta de logs si no existe
                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }

                // Escribir el mensaje de error en un archivo de texto en la carpeta de logs
                using (StreamWriter writer = new StreamWriter("logs/error.log", true))
                {
                    writer.WriteLine("Fecha: " + DateTime.Now.ToString());
                    writer.WriteLine("Mensaje de error: " + ex.Message);
                    writer.WriteLine("Stack Trace: " + ex.StackTrace);
                    writer.WriteLine("");
                }
            }
        }
    }
}
