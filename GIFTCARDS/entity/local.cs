using System.ComponentModel.DataAnnotations;

namespace GiftCards.entity
{
    public class local
    {
        [Key]
        public int id { get; set; }
        public string? descripcion { get; set; }
        public int ruc { get; set; }
        public int empresa { get; set; }
    }
}
