using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIFTCARDS.entity
{
    [Table(name: "USER_APROB_LOG")]
    public class User_aprob_log
    {
        public User_aprob_log()
        {
            this.fecha = DateTime.Now;
        }
        [Key]
        public int id { get; set; }
        public int id_cab_giftcard { get; set; }
        public int estado { get; set; }
        public int usuario { get; set; }
        public DateTime fecha {get;set;}
    }
}
