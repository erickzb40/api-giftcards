using System.ComponentModel.DataAnnotations;

namespace GiftCards.entity
{
    public class Local
    {
        [Key]
        public int id { get; set; }
        public string? descripcion { get; set; }
        public string? ruc { get; set; }
        public string? correo { get; set; }
        public string? contrasena { get; set; }
        public int puerto { get; set; }
        public string? tienda { get; set; }
    }
}
