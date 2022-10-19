

using GiftCards;
using GiftCards.entity;
using GIFTCARDS.entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asistencia_apirest.services
{
    public class util
    {
        public int[] convertirArray(List<Usuario_local> usuario_locales)
        {   

            int[] locales = new int[usuario_locales.Count()];
            var tempList = locales.ToList();
            foreach (var local in usuario_locales)
            {
                tempList = locales.ToList();
                tempList.Add(local.localid);
                locales = tempList.ToArray();
            }
            return locales;
        }
    }
}
