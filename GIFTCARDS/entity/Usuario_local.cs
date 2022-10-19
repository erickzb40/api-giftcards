using System.ComponentModel.DataAnnotations;

namespace GIFTCARDS.entity
{
    public class Usuario_local
    {
        [Key]
        public int usuarioid { get; set; }

        public int localid { get; set; }
    }
}
