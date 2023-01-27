using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftCards.entity
{
    [Table(name: "giftcardcab")]
    public class giftcardcab
    {
        public giftcardcab()
        {
            this.fecha_creacion = DateTime.Now;
        }

        [Key]
        public int id { get; set; }
        public decimal importe { get; set; }
        public int cantidad { get; set; }
        [MaxLength(50)]
        public string? nombre { get; set; }
        [MaxLength(2)]
        public string? tipo_doc { get; set; }
        public int? num_doc { get; set; }
        public int telefono { get; set; }
        [MaxLength(50)]
        public string? correo { get; set; }
        [MaxLength(20)]
        public string? documento { get; set; }
        [MaxLength(20)]
        public string? montoTexto { get; set; }
        [MaxLength(50)]
        public string? descripcion { get; set; }
        [Required]
        public int usuario { get; set; }
        [Required]
        public DateTime fecha_creacion { get; set; }
        //public int local { get; set; }
        public int estado { get; set; }
        [NotMapped]
        public DateTime fecha_vencimiento { get; set; }
        public int local { get; set; }
    }
}
