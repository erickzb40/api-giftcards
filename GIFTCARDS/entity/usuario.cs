using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftCards.entity
{
    public class Usuario
    {
        [Key]
        public int usuarioid { get; set; }
        public string? nombreusuario { get; set; }
        public string? contrasena { get; set; }
        public string? nombres { get; set; }
        public string? correoelectronico { get; set; }
        [NotMapped]
        public string empresa { get; set; }

    }
}
