

using Microsoft.EntityFrameworkCore;

namespace GiftCards.entity
{
    [Keyless]
    public class empresa
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public int? ruc { get; set; }
    }
}
