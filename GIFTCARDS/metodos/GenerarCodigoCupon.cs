using System.Text;

namespace GiftCards.metodos
{
    public class GenerarCodigoCupon
    {
        /// Encripta una cadena
        //public string Encriptar(string _cadenaAencriptar)
        //{
        //    string result = string.Empty;
        //    byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
        //    result = Convert.ToBase64String(encryted);
        //    return result;
        //}

        ///// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        //public string DesEncriptar(string _cadenaAdesencriptar)
        //{
        //    string result = string.Empty;
        //    byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
        //    //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
        //    result = System.Text.Encoding.Unicode.GetString(decryted);
        //    return result;
        //}

        const string ALPHABET = "AG8FOLE2WVTCPY5ZH3NIUDBXSMQK7946";
        public string couponCode(int number)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < 6; ++i)
            {
                b.Append(ALPHABET[(int)number & ((1 << 5) - 1)]);
                number = number >> 5;
            }
            return b.ToString();
        }
        static int codeFromCoupon(string coupon)
        {
            int n = 0;
            for (int i = 0; i < 6; ++i)
                n = n | (((int)ALPHABET.IndexOf(coupon[i])) << (5 * i));
            return n;
        }
        //const int BITCOUNT = 30; 
        //const int BITMASK = (1 << BITCOUNT / 2) - 1; 
        //static uint roundFunction(uint number) { 
        //    return (((number ^ 47894) + 25) << 1) & BITMASK; 
        //}
        //public uint crypt(uint number) { 
        //    uint left = number >> (BITCOUNT / 2); 
        //    uint right = number & BITMASK; 
        //    for (int round = 0; round < 10; ++round) { 
        //        left = left ^ roundFunction(right); 
        //        uint temp = left; 
        //        left = right; 
        //        right = temp; 
        //    } 
        //    return left | (right << (BITCOUNT / 2)); 
        //}
    }
}
