using System.ComponentModel.DataAnnotations;

namespace GiftCards.entity
{
    public class usuario
    {
        [Key]
        public int id { get; set; }
        //nombre con el que se logea el usuario
        public string usuario_nombre { get; set; }
        //nombre del usuario
        public string? nombre { get; set; }
        public string? contrasena { get; set; }
        public string? correo { get; set; }
        [Required]
        public int local { get; set; }

    }
}
