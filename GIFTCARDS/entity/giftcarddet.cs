using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftCards.entity
{
    [Table(name: "giftcarddet")]
    public class giftcarddet
    {
        [Key]
        public int id { get; set; }
        public string serie { get; set; }
        public decimal monto { get; set; }
        public string documento_ref { get; set; }
        public int id_cab { get; set; }
        public int estado { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
        public int local { get; set; }
        public DateTime? fecha_canje { get; set; }
        public int? usuario { get; set; }
        public string? mostrar { get; set; }
        //recibe como parametro para una validacion 
        [NotMapped]
        public string? nombre_usuario { get; set; }
        public string? descripcion { get; set; }
        [NotMapped]
        public string? token { get; set; }

    }

}
