using System.ComponentModel.DataAnnotations;

namespace GiftCards.entity
{
    public class estado
    {
        [Key]
        public int id { get; set; }
        [MaxLength(20)]
        public string descripcion { get; set; }
    }
}
