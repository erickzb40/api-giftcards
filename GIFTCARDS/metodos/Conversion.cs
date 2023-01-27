using GiftCards.entity;
using SelectPdf;
using System.IO.Compression;

namespace GIFTCARDS.metodos
{
    public class Conversion
    {
        public int convertirHtmlPdf(List<giftcarddet> det) {
            string htmlContent = System.IO.File.ReadAllText("img/index.txt");
            int ancho = 720;
            int alto = 480;
            foreach (giftcarddet item in det) {
                HtmlToImage hti = new HtmlToImage(ancho, alto);
                var img = hti.ConvertHtmlString(htmlContent);
                img.Save("img_temp/"+item.serie+".jpg");
            }
            int result=this.comprimir();
            return result;

        }
        public int comprimir() {
            string folderPath = "img_temp/";
            string[] imgFiles = Directory.GetFiles(folderPath, "*.jpg");
            string rarFile = "rar_temp/giftcards.zip";
            using (FileStream zipToOpen = new FileStream(rarFile, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    foreach (string file in imgFiles)
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileName(file));
                    }
                }
            }
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            return imgFiles.Length;
        }
    }
}
