using System;
using System.Globalization;
using System.Text;

namespace TestFonctions
{
    public static class GestionHexa
    {

        public static string ToHexString(this byte[] hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return string.Empty;

            var s = new StringBuilder();
            foreach (byte b in hex)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }

        public static string ByteToHexString(this byte[] hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return string.Empty;

            string hexString = BitConverter.ToString(hex);
            string HexOutput = hexString.ToLower().Replace("-", "");
            return HexOutput;
        }

        public static byte[] ToHexBytes(this string hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return new byte[0];

            int l = hex.Length / 2;
            var b = new byte[l];
            for (int i = 0; i < l; ++i)
            {
                b[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return b;
        }

        public static bool EqualsTo(this byte[] bytes, byte[] bytesToCompare)
        {
            if (bytes == null && bytesToCompare == null) return true; // ?
            if (bytes == null || bytesToCompare == null) return false;
            if (object.ReferenceEquals(bytes, bytesToCompare)) return true;

            if (bytes.Length != bytesToCompare.Length) return false;

            for (int i = 0; i < bytes.Length; ++i)
            {
                if (bytes[i] != bytesToCompare[i]) return false;
            }
            return true;
        }
        public static string HEX2ASCII(string hex)

        {

            string res = String.Empty;

            for (int a = 0; a < hex.Length; a = a + 2)

            {
                string Char2Convert = hex.Substring(a, 2);

                int n = Convert.ToInt32(Char2Convert, 16);

                char c = (char)n;

                res += c.ToString();
            }
            return res;
        }

        public static string ASCIITOHex(string ascii)

        {
            StringBuilder sb = new StringBuilder();

            byte[] inputBytes = Encoding.UTF8.GetBytes(ascii);

            foreach (byte b in inputBytes)
            {
                sb.Append(string.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
        public static int GetHexVal(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        public static void HexaExtractLines(byte[] RecordHexa, ref string HexaFirstLine, ref string HexaSecondLine)
        {
            string ligneHexa = BitConverter.ToString(RecordHexa);

            HexaFirstLine = string.Empty;
            HexaSecondLine = string.Empty;

            int ResteDiv = 0;

            for (int i = 0; i < ligneHexa.Length; ++i)
            {
                ResteDiv = i % 3;

                switch (ResteDiv)
                {
                    case 0:
                        HexaFirstLine = HexaFirstLine + ligneHexa[i];
                        break;
                    case 1:
                        HexaSecondLine = HexaSecondLine + ligneHexa[i];
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
